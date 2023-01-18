using System.Collections.Immutable;
using Dapper;

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

    public DevicesStorage(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task<DeviceDto?> CreateDeviceAsync(DeviceDto deviceDto)
    {
        using var connection = _dapperContext.CreateConnection();
        var insertedCount = await connection.ExecuteAsync($@"
insert into {DapperContext.DevicesTable} (device_id, name, location) values
(@deviceId, @name, @location)", new
        {
            deviceId = deviceDto.DeviceId,
            name = deviceDto.Name,
            location = deviceDto.Location,
        });

        return insertedCount != 1 ? null : await GetDeviceByIdAsync(deviceDto.DeviceId);
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
        using var connection = _dapperContext.CreateConnection();
        var updatedCount = await connection.ExecuteAsync($@"
update {DapperContext.DevicesTable}
set device_id = @newDeviceId, name = @newName, location = @newLocation,
where device_id = @deviceId
".Trim(), new
        {
            deviceId,
            newDeviceId = deviceDto.DeviceId,
            newName = deviceDto.Name,
            newLocation = deviceDto.Location,
        });

        return updatedCount == 0 ? null : await GetDeviceByIdAsync(deviceDto.DeviceId);
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