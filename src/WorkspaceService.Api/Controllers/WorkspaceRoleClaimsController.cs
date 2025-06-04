using Microsoft.AspNetCore.Mvc;
using WorkspaceService.Api.Controllers.Base;
using WorkspaceService.Domain.DTOs;
using WorkspaceService.Domain.DTOs.WorkspaceRoleClaims;
using WorkspaceService.Domain.DTOs.WorkspaceRoles;
using WorkspaceService.Domain.Services;

namespace WorkspaceService.Api.Controllers;

public class WorkspaceRoleClaimsController : BaseController<WorkspaceRoleClaimsController>
{
    private readonly IWorkspaceRoleClaimsService _workspaceRoleClaimsService;

    public WorkspaceRoleClaimsController(IWorkspaceRoleClaimsService 
            workspaceRoleClaimsService, ILogger<WorkspaceRoleClaimsController> logger): base(logger)
    {
        _workspaceRoleClaimsService = workspaceRoleClaimsService;
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<RoleClaimsDto>> GetAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(GetAsync));
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
        LogRequest(nameof(GetListAsync));
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
        LogRequest(nameof(CreateAsync));
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
        LogRequest(nameof(UpdateAsync));
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
        LogRequest(nameof(DeleteAsync));
        await _workspaceRoleClaimsService.DeleteAsync(id, cancellationToken);
        return Ok();
    }
}