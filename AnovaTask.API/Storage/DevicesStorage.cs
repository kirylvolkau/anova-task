using System.Collections.Immutable;

namespace AnovaTask.API.Storage;

public interface IDevicesStorage
{
    public Task<Device> CreateDeviceAsync(Device device);
    public Task<Device> GetDeviceByIdAsync(int deviceId);
    public Task<ImmutableList<Device>> GetAllDevicesAsync(int deviceId);
    public Task<Device> UpdateDeviceAsync(Device device);
    public Task<Device> DeleteDeviceByIdAsync(int deviceId);
}

public class DevicesStorage
{
    
}