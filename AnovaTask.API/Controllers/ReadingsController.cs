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
    public async Task<IActionResult> InsertReadingsAsync([FromBody] ImmutableList<ReadingDto> readingDtos)
    {
        var insertedLines = await _readingsStorage.AddReadingsAsync(readingDtos);
        
        return insertedLines ? Ok() : BadRequest("Couldn't insert all readings.");
    }

    [HttpGet("{deviceId:int}/{from:long}/{to:long?}")]
    public async Task<IActionResult> GetReadingsForDeviceAsync(int deviceId, long from, long? to = null)
    {
        to ??= DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        if (to < from)
        {
            return BadRequest("Time window is incorrect: opening time is less than closing time.");
        }

        var readings = await _readingsStorage.GetReadingsFromWindowAsync(deviceId, from, to.Value);
        
        return readings is not null ? Ok(readings) : NotFound($"Device with ID {deviceId} doesn't exist.");
    }
}