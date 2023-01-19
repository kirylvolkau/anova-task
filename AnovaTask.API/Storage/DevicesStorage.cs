using System.Collections.Immutable;
using Dapper;
using Npgsql;

namespace AnovaTask.API.Storage;

public interface IDevicesStorage
{
    public Task<DeviceDto?> CreateDeviceAsync(DeviceDto deviceDto);
    public Task<DeviceDto?> GetDeviceByIdAsync(int deviceId);
    public Task<ImmutableList<DeviceDto>> GetAllDevicesAsync();
    public Task<DeviceDto?> UpdateDeviceAsync(int deviceId, DeviceDto deviceDto);
    public Task<DeviceDto?> DeleteDeviceByIdAsync(int deviceId);
}

public class DevicesStorage : IDevicesStorage
{
    private readonly DapperContext _dapperContext;
    private readonly ILogger<DevicesStorage> _logger;

    public DevicesStorage(DapperContext dapperContext, ILogger<DevicesStorage> logger)
    {
        _dapperContext = dapperContext;
        _logger = logger;
    }

    public async Task<DeviceDto?> CreateDeviceAsync(DeviceDto deviceDto)
    {
        try
        {
            using var connection = _dapperContext.CreateConnection();
            _ = await connection.ExecuteAsync($@"
insert into {DapperContext.DevicesTable} (device_id, name, location) values
(@deviceId, @name, @location)", new
            {
                deviceId = deviceDto.DeviceId,
                name = deviceDto.Name,
                location = deviceDto.Location,
            });
        }
        catch (PostgresException e)
        {
            _logger.LogError(e.MessageText);
            return null;
        }

        return await GetDeviceByIdAsync(deviceDto.DeviceId);
    }

    public async Task<DeviceDto?> GetDeviceByIdAsync(int deviceId)
    {
        using var connection = _dapperContext.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<DeviceDto?>(
            $"select * from {DapperContext.DevicesTable} where device_id = @deviceId",
            new { deviceId });
    }

    public async Task<ImmutableList<DeviceDto>> GetAllDevicesAsync()
    {
        using var connection = _dapperContext.CreateConnection();
        var devices = await connection.QueryAsync<DeviceDto>($"select * from {DapperContext.DevicesTable}") ?? new List<DeviceDto>();

        return devices.ToImmutableList();
    }

    public async Task<DeviceDto?> UpdateDeviceAsync(int deviceId, DeviceDto deviceDto)
    {
        try
        {
            using var connection = _dapperContext.CreateConnection();

            var device = await GetDeviceByIdAsync(deviceId);
            if (device is null)
            {
                return null;
            }

            _ = await connection.ExecuteAsync($@"
update {DapperContext.DevicesTable}
set device_id = @newDeviceId, name = @newName, location = @newLocation
where device_id = @deviceId
".Trim(), new
            {
                deviceId,
                newDeviceId = deviceDto.DeviceId,
                newName = deviceDto.Name,
                newLocation = deviceDto.Location,
            });
        }
        catch (PostgresException e)
        {
            _logger.LogError(e.MessageText);
            return null;
        }

        return await GetDeviceByIdAsync(deviceDto.DeviceId);
    }

    public async Task<DeviceDto?> DeleteDeviceByIdAsync(int deviceId)
    {
        var device = await GetDeviceByIdAsync(deviceId);
        if (device is null)
        {
            return null;
        }

        using var connection = _dapperContext.CreateConnection();
        var deletedCount = await connection.ExecuteAsync(
            $"delete from {DapperContext.DevicesTable} where device_id = @deviceId",
            new { deviceId });

        return deletedCount == 0 ? null : device;
    }
}