using AnovaTask.API.Storage;
using Microsoft.AspNetCore.Mvc;

namespace AnovaTask.API.Controllers;

[ApiController]
[Route("/readings/{deviceId:int}")]
public class ReadingsController
{
    private readonly IReadingsStorage _readingsStorage;
    
    [FromRoute]
    public int DeviceId { get; set; }

    public ReadingsController(IReadingsStorage readingsStorage)
    {
        _readingsStorage = readingsStorage;
    }
}