using System.Collections.Immutable;
using AnovaTask.API.Storage;
using Microsoft.AspNetCore.Mvc;

namespace AnovaTask.API.Controllers;

/// <summary>
/// Controller for Readings.
/// </summary>
[ApiController]
[Route("/readings")]
public class ReadingsController : ControllerBase
{
    private readonly IReadingsStorage _readingsStorage;

    /// <summary>
    /// Constructor for readings.
    /// </summary>
    /// <param name="readingsStorage">Implementatino of <see cref="IReadingsStorage"/></param>
    public ReadingsController(IReadingsStorage readingsStorage)
    {
        _readingsStorage = readingsStorage;
    }

    /// <summary>
    /// Inserts readings to the database.
    /// </summary>
    /// <param name="readingDtos">List of DTOs to be saved.</param>
    /// <response code="200">Empty body in case of success</response>
    /// <response code="400">If there was any kind of error inserting readings</response>
    [HttpPost]
    public async Task<IActionResult> InsertReadingsAsync([FromBody] ImmutableList<ReadingDto> readingDtos)
    {
        var insertedLines = await _readingsStorage.AddReadingsAsync(readingDtos);

        return insertedLines ? Ok() : BadRequest("Couldn't insert all readings.");
    }

    /// <summary>
    /// Get readings in the time window provided for given device.
    /// </summary>
    /// <param name="deviceId">ID of the device to get readings for.</param>
    /// <param name="from">Opening timestamp of the time window (in seconds from unix epoch).</param>
    /// <param name="to">Closing timestamp of the time window (in seconds from unix epoch.)</param>
    /// <returns>Readings for the device from time window.</returns>
    /// <response code="200">Readings for the device from time window.</response>
    /// <response code="404">Error if device was not found.</response>
    /// <response code="400">Error in case the time window was incorrect.</response>
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