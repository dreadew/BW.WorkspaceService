using Microsoft.AspNetCore.Mvc;
using WorkspaceService.Domain.DTOs;
using WorkspaceService.Domain.DTOs.WorkspaceDirectory;
using WorkspaceService.Domain.DTOs.Workspaces;
using WorkspaceService.Domain.Services;

namespace WorkspaceService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WorkspaceDirectoryController : ControllerBase
{
    private readonly IWorkspaceDirectoryService _workspaceDirectoryService;
    private readonly ILogger<WorkspaceRoleClaimsController> _logger;

    public WorkspaceDirectoryController(IWorkspaceDirectoryService workspaceDirectoryService,
        ILogger<WorkspaceRoleClaimsController> logger)
    {
        _workspaceDirectoryService = workspaceDirectoryService;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<DirectoryDto>> GetAsync(string id,
        CancellationToken cancellationToken = default)
    {
        var result = await _workspaceDirectoryService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpGet("list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DirectoryDto>>> ListAsync(
        [FromQuery] ListRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspaceId = HttpContext.Request.Headers["WorkspaceId"].FirstOrDefault();
        if (string.IsNullOrEmpty(workspaceId) || !Guid.TryParse(workspaceId, out _))
        {
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }
        
        var result = await _workspaceDirectoryService.ListAsync(dto, workspaceId,
            cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateAsync([FromBody] CreateDirectoryRequest dto,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }
        
        await _workspaceDirectoryService.CreateAsync(dto, cancellationToken);
        return Ok();
    }

    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateAsync([FromBody] UpdateDirectoryRequest dto,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }
        
        await _workspaceDirectoryService.UpdateAsync(dto, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteAsync(string id,
        CancellationToken cancellationToken = default)
    {
        await _workspaceDirectoryService.DeleteAsync(id, cancellationToken);
        return Ok();
    }
}