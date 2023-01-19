using System.Collections.Immutable;
using Dapper;
using Npgsql;

namespace AnovaTask.API.Storage;

public interface IReadingsStorage
{
    public Task<bool> AddReadingsAsync(IEnumerable<ReadingDto> readings);

    public Task<ImmutableList<ReadingDto>?> GetReadingsFromWindowAsync(int deviceId, long from, long to);
}

public class ReadingsStorage : IReadingsStorage
{
    private readonly DapperContext _dapperContext;
    private readonly ILogger<ReadingsStorage> _logger;

    public ReadingsStorage(DapperContext dapperContext, ILogger<ReadingsStorage> logger)
    {
        _dapperContext = dapperContext;
        _logger = logger;
    }

    public async Task<bool> AddReadingsAsync(IEnumerable<ReadingDto> readings)
    {
        using var connection = _dapperContext.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();
        try
        {
            foreach (var reading in readings)
            {
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