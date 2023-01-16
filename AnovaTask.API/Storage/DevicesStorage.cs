using System.Collections.Immutable;

namespace AnovaTask.API.Storage;

public interface IDevicesStorage
{
    public Task<DeviceDto?> CreateDeviceAsync(DeviceDto deviceDto);
    public Task<DeviceDto?> GetDeviceByIdAsync(int deviceId);
    public Task<ImmutableList<DeviceDto>> GetAllDevicesAsync(int deviceId);
    public Task<DeviceDto?> UpdateDeviceAsync(DeviceDto deviceDto);
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

    public Task<ImmutableList<DeviceDto>> GetAllDevicesAsync(int deviceId)
    {
        throw new NotImplementedException();
    }

    public Task<DeviceDto?> UpdateDeviceAsync(DeviceDto deviceDto)
    {
        throw new NotImplementedException();
    }

    public Task<DeviceDto?> DeleteDeviceByIdAsync(int deviceId)
    {
        throw new NotImplementedException();
    }
}