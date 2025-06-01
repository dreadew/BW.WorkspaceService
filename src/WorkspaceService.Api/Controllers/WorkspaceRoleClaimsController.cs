using Microsoft.AspNetCore.Mvc;
using WorkspaceService.Domain.DTOs;
using WorkspaceService.Domain.DTOs.WorkspaceRoleClaims;
using WorkspaceService.Domain.DTOs.WorkspaceRoles;
using WorkspaceService.Domain.Services;

namespace WorkspaceService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkspaceRoleClaimsController : ControllerBase
{
    private readonly IWorkspaceRoleClaimsService _workspaceRoleClaimsService;
    private readonly ILogger<WorkspaceRoleController> _logger;

    public WorkspaceRoleClaimsController(IWorkspaceRoleClaimsService 
            workspaceRoleClaimsService,
        ILogger<WorkspaceRoleController> logger)
    {
        _workspaceRoleClaimsService = workspaceRoleClaimsService;
        _logger = logger;
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<RoleClaimsDto>> GetAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _workspaceRoleClaimsService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{roleId:guid}/list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetListAsync(
        Guid roleId,
        [FromQuery] ListRequest dto,
        CancellationToken cancellationToken = default)
    {
        var result = await _workspaceRoleClaimsService.ListAsync(dto, roleId, 
            cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateAsync(
        CreateRoleClaimsRequest dto,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }

        await _workspaceRoleClaimsService.CreateAsync(dto, cancellationToken);
        return Ok();
    }

    [HttpPatch("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateAsync(
        [FromBody] UpdateRoleClaimsRequest dto,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }
        
        await _workspaceRoleClaimsService.UpdateAsync(dto, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        await _workspaceRoleClaimsService.DeleteAsync(id, cancellationToken);
        return Ok();
    }
}