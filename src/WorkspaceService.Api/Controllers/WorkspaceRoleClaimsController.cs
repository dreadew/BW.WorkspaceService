using Microsoft.AspNetCore.Mvc;

namespace WorkspaceService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkspaceRoleClaimsController : ControllerBase
{
    private readonly ILogger<WorkspaceRoleClaimsController> _logger;
}