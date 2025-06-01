using Microsoft.AspNetCore.Mvc;
using WorkspaceService.Domain.DTOs;
using WorkspaceService.Domain.DTOs.WorkspaceRoles;
using WorkspaceService.Domain.Services;

namespace WorkspaceService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkspaceRoleController : ControllerBase
{
    private readonly IWorkspaceRolesService _workspaceRolesService;
    private readonly ILogger<WorkspaceRoleController> _logger;

    public WorkspaceRoleController(IWorkspaceRolesService workspaceRolesService,
        ILogger<WorkspaceRoleController> logger)
    {
        _workspaceRolesService = workspaceRolesService;
        _logger = logger;
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<RoleDto>> GetAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _workspaceRolesService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{workspaceId:guid}/list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetListAsync(
        Guid workspaceId,
        [FromQuery] ListRequest dto,
        CancellationToken cancellationToken = default)
    {
        var result = await _workspaceRolesService.ListAsync(dto, workspaceId, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateAsync(
        CreateRoleRequest dto,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }

        await _workspaceRolesService.CreateAsync(dto, cancellationToken);
        return Ok();
    }

    [HttpPatch("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateAsync(
        UpdateRoleRequest dto,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }
        
        await _workspaceRolesService.UpdateAsync(dto, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        await _workspaceRolesService.DeleteAsync(id, cancellationToken);
        return Ok();
    }
    
    [HttpPost("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> RestoreAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        await _workspaceRolesService.RestoreAsync(id, cancellationToken);
        return Ok();
    }
}