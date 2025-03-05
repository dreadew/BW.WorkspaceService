using Microsoft.AspNetCore.Mvc;
using WorkspaceService.Domain.DTOs;
using WorkspaceService.Domain.DTOs.Workspaces;
using WorkspaceService.Domain.DTOs.WorkspaceUsers;
using WorkspaceService.Domain.Services;

namespace WorkspaceService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WorkspaceController : ControllerBase
{
    private readonly IWorkspaceService _workspaceService;
    private readonly ILogger<WorkspaceController> _logger;

    public WorkspaceController(IWorkspaceService workspaceService,
        ILogger<WorkspaceController> logger)
    {
        _workspaceService = workspaceService;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateAsync(
        [FromBody] CreateWorkspaceRequest dto,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }
        
        await _workspaceService.CreateAsync(dto, cancellationToken);
        return Ok();
    }
    
    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateAsync(
        [FromBody] UpdateWorkspaceRequest dto,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }
        
        await _workspaceService.UpdateAsync(dto, cancellationToken);
        return Ok();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<WorkspaceDto>> GetAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        var result = await _workspaceService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpGet("list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<WorkspaceDto>>> ListAsync(
        [FromQuery] ListRequest dto,
        CancellationToken cancellationToken = default)
    {
        var userId = HttpContext.Request.Headers["UserId"].FirstOrDefault();
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out _))
        {
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }
        
        var result = await _workspaceService.ListAsync(dto, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        var userId = HttpContext.Request.Headers["UserId"].FirstOrDefault();
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out _))
        {
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }
        
        await _workspaceService.DeleteAsync(id, cancellationToken);
        return Ok();
    }

    [HttpPost("invite-user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> InviteToWorkspaceAsync(
        InviteUserRequest dto,
        CancellationToken cancellationToken = default)
    {
        await _workspaceService.InviteUserAsync(dto, cancellationToken);
        return Ok();
    }
    
    [HttpPatch("update-user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateUserAsync(
        UpdateUserRequest dto,
        CancellationToken cancellationToken = default)
    {
        await _workspaceService.UpdateUserAsync(dto, cancellationToken);
        return Ok();
    }
    
    [HttpPost("delete-user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteFromWorkspaceAsync(
        DeleteUserRequest dto,
        CancellationToken cancellationToken = default)
    {
        await _workspaceService.DeleteUserAsync(dto, cancellationToken);
        return Ok();
    }
}