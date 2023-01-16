using Microsoft.AspNetCore.Mvc;

namespace AnovaTask.API.Controllers;

[ApiController]
[Route("/readings/{deviceId:int}")]
public class ReadingsController
{
    [FromRoute]
    public int DeviceId { get; set; }
    
    
}