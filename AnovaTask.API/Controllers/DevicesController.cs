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
}