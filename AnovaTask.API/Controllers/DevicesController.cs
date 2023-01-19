using System.Collections.Immutable;
using AnovaTask.API.Storage;
using Microsoft.AspNetCore.Mvc;

namespace AnovaTask.API.Controllers;

/// <summary>
/// Controller for Devices.
/// </summary>
[ApiController]
[Route("/device")]
public class DevicesController : ControllerBase
{
    private readonly IDevicesStorage _devicesStorage;

    /// <summary>
    /// Contstructor of the device controller.
    /// </summary>
    /// <param name="devicesStorage">Implementation of <see cref="IDevicesStorage"/></param>
    public DevicesController(IDevicesStorage devicesStorage)
    {
        _devicesStorage = devicesStorage;
    }

    /// <summary>
    /// Creates device from provided DTO.
    /// </summary>
    /// <param name="deviceDto">Device to be created.</param>
    /// <returns>Device created.</returns>
    /// <response code="200">Device created</response>
    /// <response code="400">Errors of device dto / device with such ID exists already</response>
    [HttpPost]
    public async Task<IActionResult> CreateDeviceAsync([FromBody] DeviceDto deviceDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var device = await _devicesStorage.CreateDeviceAsync(deviceDto);

        return device is not null
            ? Ok(device)
            : BadRequest($"Device with ID {deviceDto.DeviceId} already exists.");
    }

    /// <summary>
    /// Gets device with provided ID. 
    /// </summary>
    /// <param name="deviceId">ID of the device to get.</param>
    /// <returns>Device DTO</returns>
    /// <response code="200">Device with ID provided</response>
    /// <response code="404">If no such device found</response>
    [HttpGet("/{deviceId:int}")]
    public async Task<IActionResult> GetDeviceByIdAsync(int deviceId)
    {
        var device = await _devicesStorage.GetDeviceByIdAsync(deviceId);

        return device is not null ? Ok(device) : NotFound($"Device with Id {deviceId} doesn't exist.");
    }

    /// <summary>
    /// Gets all devices.
    /// </summary>
    /// <returns>All devices.</returns>
    /// <response code="200">All devices.</response>
    [HttpGet]
    public async Task<IActionResult> GetAllDevicesAsync()
    {
        return Ok(await _devicesStorage.GetAllDevicesAsync());
    }

    /// <summary>
    /// Updates device (found by ID) with data provided in the DTO.
    /// </summary>
    /// <param name="deviceId">ID of the device to update.</param>
    /// <param name="deviceDto">New values of the updated device.</param>
    /// <returns>Updated device</returns>
    [HttpPut("/{deviceId:int}")]
    public async Task<IActionResult> UpdateDeviceAsync(int deviceId, [FromBody] DeviceDto deviceDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var device = await _devicesStorage.UpdateDeviceAsync(deviceId, deviceDto);

        return device is not null ? Ok(device) : NotFound($"Device with Id {deviceId} doesn't exist.");
    }

    /// <summary>
    /// Deletes device by ID provided.
    /// </summary>
    /// <param name="deviceId">ID of the device to delete.</param>
    /// <returns>Deleted device (if any).</returns>
    /// <response code="200">Device deleted</response>
    /// <response code="404">If no such device found</response>
    [HttpDelete("/{deviceId:int}")]
    public async Task<IActionResult> DeleteDeviceAsync([FromRoute] int deviceId)
    {
        var device = await _devicesStorage.DeleteDeviceByIdAsync(deviceId);

        return device is not null ? Ok(device) : NotFound($"Device with Id {deviceId} doesn't exist.");
    }
}