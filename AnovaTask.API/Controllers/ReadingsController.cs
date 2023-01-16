using System.Collections.Immutable;
using AnovaTask.API.Storage;
using Microsoft.AspNetCore.Mvc;

namespace AnovaTask.API.Controllers;

[ApiController]
[Route("/readings/{deviceId:int}")]
public class ReadingsController : ControllerBase
{
    private readonly IReadingsStorage _readingsStorage;
    
    [FromRoute]
    public int DeviceId { get; set; }

    public ReadingsController(IReadingsStorage readingsStorage)
    {
        _readingsStorage = readingsStorage;
    }

    [HttpPost]
    public async Task<IActionResult> AssignReadingsToDeviceAsync([FromBody] ImmutableList<ReadingDto> readingDtos)
    {
        var insertedLines = await _readingsStorage.AddReadingsToDeviceAsync(DeviceId, readingDtos);
        
        return insertedLines is null ? Ok() : NotFound($"Device with ID {DeviceId} doesn't exist.");
    }

    [HttpGet("/{from:long}/{to:long?}")]
    public async Task<IActionResult> GetReadingsForDeviceAsync([FromRoute] long from, [FromRoute] long? to = null)
    {
        to ??= DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var readings = await _readingsStorage.GetReadingsFromWindowAsync(DeviceId, from, to.Value);
        
        return readings is not null ? Ok(readings) : NotFound($"Device with ID {DeviceId} doesn't exist.");
    }
}