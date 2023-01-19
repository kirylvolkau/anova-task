using System.Collections.Immutable;
using AnovaTask.API.Storage;
using Microsoft.AspNetCore.Mvc;

namespace AnovaTask.API.Controllers;

[ApiController]
[Route("/device")]
public class DevicesController : ControllerBase
{
    private readonly IDevicesStorage _devicesStorage;

    public DevicesController(IDevicesStorage devicesStorage)
    {
        _devicesStorage = devicesStorage;
    }

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

    [HttpGet("/{deviceId:int}")]
    public async Task<IActionResult> GetDeviceByIdAsync(int deviceId)
    {
        var device = await _devicesStorage.GetDeviceByIdAsync(deviceId);

        return device is not null ? Ok(device) : NotFound($"Device with Id {deviceId} doesn't exist.");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDevicesAsync()
    {
        return Ok(await _devicesStorage.GetAllDevicesAsync());
    }

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

    [HttpDelete("/{deviceId:int}")]
    public async Task<IActionResult> DeleteDeviceAsync([FromRoute] int deviceId)
    {
        var device = await _devicesStorage.DeleteDeviceByIdAsync(deviceId);

        return device is not null ? Ok(device) : NotFound($"Device with Id {deviceId} doesn't exist.");
    }
}