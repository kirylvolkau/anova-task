using System.Collections.Immutable;
using Dapper;

namespace AnovaTask.API.Storage;

public interface IReadingsStorage
{
    public Task<int> AddReadingsAsync(IEnumerable<ReadingDto> readings);

    public Task<ImmutableList<ReadingDto>?> GetReadingsFromWindowAsync(int deviceId, long from, long to);
}

public class ReadingsStorage : IReadingsStorage
{
    private readonly DapperContext _dapperContext;

    public ReadingsStorage(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task<int> AddReadingsAsync(IEnumerable<ReadingDto> readings)
    {
        using var connection = _dapperContext.CreateConnection();
        var insertedCount = 0;
        foreach (var reading in readings)
        {
            var inserted = await connection.ExecuteAsync($@"
insert into {DapperContext.ReadingsTable} (timestamp, device_id, reading_type, raw_value) values 
(@timestamp, @device_id, @reading_type, @raw_value)".Trim(), new
            {
                timestamp = reading.Timestamp,
                device_id = reading.DeviceId,
                reading_type = reading.ReadingType,
                raw_value = reading.RawValue,
            });
            insertedCount += inserted;
        }

        return insertedCount;
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
            deviceId, from, to,
        });

        return readings.ToImmutableList();
    }
}