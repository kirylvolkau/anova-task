using System.Collections.Immutable;
using Dapper;
using Npgsql;

namespace AnovaTask.API.Storage;

/// <summary>
/// Data access layer for the Readings data.
/// </summary>
public interface IReadingsStorage
{
    /// <summary>
    /// Saves provided collection of readings to the database. 
    /// </summary>
    /// <param name="readings">Collection of readings (possible from multiple devices).</param>
    /// <returns>Indication of operation success / failure</returns>
    public Task<bool> AddReadingsAsync(IEnumerable<ReadingDto> readings);

    /// <summary>
    /// Finds all readings for given device and time window.
    /// </summary>
    /// <param name="deviceId">Id of the device</param>
    /// <param name="from">Opening timestamp of the time window</param>
    /// <param name="to">Closing timestamp of the time window</param>
    /// <returns>Collection of <see cref="ReadingDto"/> or null (in case device not found).</returns>
    public Task<ImmutableList<ReadingDto>?> GetReadingsFromWindowAsync(int deviceId, long from, long to);
}

/// <inheritdoc />
public class ReadingsStorage : IReadingsStorage
{
    private readonly DapperContext _dapperContext;
    private readonly ILogger<ReadingsStorage> _logger;

    /// <summary>
    /// Constructor to instantiate storage of readings.
    /// </summary>
    public ReadingsStorage(DapperContext dapperContext, ILogger<ReadingsStorage> logger)
    {
        _dapperContext = dapperContext;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<bool> AddReadingsAsync(IEnumerable<ReadingDto> readings)
    {
        using var connection = _dapperContext.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();
        try
        {
            foreach (var reading in readings)
            {
                // unfortunately, Dapper doesn't have the batch update functionality (without SQL injection, obviously)
                // one of possible solutions is:
                // 1. serializing collection to be inserted as json
                // 2. passing this json as an argument to SQL query
                // 3. deserializing the json inside the query itself
                _ = await connection.ExecuteAsync($@"
insert into {DapperContext.ReadingsTable} (timestamp, device_id, reading_type, raw_value) values 
(@timestamp, @device_id, @reading_type, @raw_value)".Trim(), new
                {
                    timestamp = reading.Timestamp,
                    device_id = reading.DeviceId,
                    reading_type = reading.ReadingType,
                    raw_value = reading.RawValue,
                });
            }
            transaction.Commit();
            return true;
        }
        catch (PostgresException ex)
        {
            _logger.LogError(ex.MessageText);
            transaction.Rollback();
            return false;
        }
    }

    /// <inheritdoc />
    public async Task<ImmutableList<ReadingDto>?> GetReadingsFromWindowAsync(int deviceId, long from, long to)
    {
        using var connection = _dapperContext.CreateConnection();
        var device = await connection.QueryFirstOrDefaultAsync<DeviceDto?>(
            $"select * from {DapperContext.DevicesTable} where device_id = @deviceId",
            new { deviceId });
        if (device is null)
        {
            return null;
        }

        var readings = await connection.QueryAsync<ReadingDto>($@"
select * from {DapperContext.ReadingsTable}
where device_id = @deviceId
and timestamp between @from and @to
".Trim(), new
        {
            deviceId,
            from,
            to,
        });

        return readings.ToImmutableList();
    }
}