using System.Collections.Immutable;
using Dapper;
using Npgsql;

namespace AnovaTask.API.Storage;

/// <summary>
/// Data access layer for the Devices data.
/// </summary>
public interface IDevicesStorage
{
    /// <summary>
    /// Creates new device.
    /// </summary>
    /// <param name="deviceDto">Device DTO to create a new device</param>
    /// <returns>Device if creation was successful, otherwise null</returns>
    public Task<DeviceDto?> CreateDeviceAsync(DeviceDto deviceDto);

    /// <summary>
    /// Get device by ID.
    /// </summary>
    /// <param name="deviceId">Device ID</param>
    /// <returns>Device with the ID provided (or null, if missing)</returns>
    public Task<DeviceDto?> GetDeviceByIdAsync(int deviceId);

    /// <summary>
    /// Get all devices.
    /// </summary>
    /// <returns>Collection of all devices present</returns>
    public Task<ImmutableList<DeviceDto>> GetAllDevicesAsync();

    /// <summary>
    /// Updates device with given ID with provided <see cref="DeviceDto"/>. 
    /// </summary>
    /// <param name="deviceId">ID of the device to be updated</param>
    /// <param name="deviceDto">New data for the device</param>
    /// <returns>New value of the update device (or null if no device to update was found)</returns>
    public Task<DeviceDto?> UpdateDeviceAsync(int deviceId, DeviceDto deviceDto);

    /// <summary>
    /// Deleted device with provided ID.
    /// </summary>
    /// <param name="deviceId">ID of the device deleted.</param>
    /// <returns>Device delete (null if couldn't find it).</returns>
    public Task<DeviceDto?> DeleteDeviceByIdAsync(int deviceId);
}

/// <inheritdoc />
public class DevicesStorage : IDevicesStorage
{
    private readonly DapperContext _dapperContext;
    private readonly ILogger<DevicesStorage> _logger;

    /// <summary>
    /// Constructor to instantiate storage of devices.
    /// </summary>
    public DevicesStorage(DapperContext dapperContext, ILogger<DevicesStorage> logger)
    {
        _dapperContext = dapperContext;
        _logger = logger;
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<DeviceDto?> GetDeviceByIdAsync(int deviceId)
    {
        using var connection = _dapperContext.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<DeviceDto?>(
            $"select * from {DapperContext.DevicesTable} where device_id = @deviceId",
            new { deviceId });
    }

    /// <inheritdoc />
    public async Task<ImmutableList<DeviceDto>> GetAllDevicesAsync()
    {
        using var connection = _dapperContext.CreateConnection();
        var devices = await connection.QueryAsync<DeviceDto>($"select * from {DapperContext.DevicesTable}") ?? new List<DeviceDto>();

        return devices.ToImmutableList();
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
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