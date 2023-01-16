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
    
    public Task<DeviceDto?> CreateDeviceAsync(DeviceDto deviceDto)
    {
        throw new NotImplementedException();
    }

    public Task<DeviceDto?> GetDeviceByIdAsync(int deviceId)
    {
        throw new NotImplementedException();
    }

    public async Task<ImmutableList<DeviceDto>> GetAllDevicesAsync()
    {
        var commandText = $"select * from devices";
        using var connection = _dapperContext.CreateConnection();
        var devices = await connection.QueryAsync<DeviceDto>(commandText) ?? new List<DeviceDto>();

        return devices.ToImmutableList();
    }

    public Task<DeviceDto?> UpdateDeviceAsync(int deviceId, DeviceDto deviceDto)
    {
        throw new NotImplementedException();
    }

    public Task<DeviceDto?> DeleteDeviceByIdAsync(int deviceId)
    {
        throw new NotImplementedException();
    }
}