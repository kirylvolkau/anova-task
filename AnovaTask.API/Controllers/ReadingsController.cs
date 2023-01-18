using System.Collections.Immutable;
using AnovaTask.API.Storage;
using Microsoft.AspNetCore.Mvc;

namespace AnovaTask.API.Controllers;

[ApiController]
[Route("/readings")]
public class ReadingsController : ControllerBase
{
    private readonly IReadingsStorage _readingsStorage;

    public ReadingsController(IReadingsStorage readingsStorage)
    {
        _readingsStorage = readingsStorage;
    }

    [HttpPost]
    public async Task<IActionResult> AssignReadingsToDeviceAsync([FromBody] ImmutableList<ReadingDto> readingDtos)
    {
        var insertedLines = await _readingsStorage.AddReadingsAsync(readingDtos);
        
        return insertedLines == 0 ? BadRequest("Couldn't insert all readings.") : Ok();
    }

    [HttpGet("{deviceId:int}/{from:long}/{to:long?}")]
    public async Task<IActionResult> GetReadingsForDeviceAsync(
        [FromRoute] int deviceId,
        [FromRoute] long from,
        [FromRoute] long? to = null)
    {
        to ??= DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var readings = await _readingsStorage.GetReadingsFromWindowAsync(deviceId, from, to.Value);
        
        return readings is not null ? Ok(readings) : NotFound($"Device with ID {deviceId} doesn't exist.");
    }
}