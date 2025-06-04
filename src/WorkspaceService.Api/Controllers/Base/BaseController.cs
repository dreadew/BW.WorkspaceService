using Microsoft.AspNetCore.Mvc;

namespace WorkspaceService.Api.Controllers.Base;

[ApiController]
[Route("api/[controller]")]
public class BaseController<T> : ControllerBase
{
    protected ILogger<T> Logger { get; }

    public BaseController(ILogger<T> logger)
    {
        Logger = logger;
    }
    
    protected void LogRequest(string actionName)
    {
        var user = HttpContext?.User?.Identity?.Name ?? "Неизвестно";
        Logger.LogInformation("{Action} вызван {User}", actionName, user);
    }
}