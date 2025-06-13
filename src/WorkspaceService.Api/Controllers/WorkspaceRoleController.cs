using Common.AspNetCore.Controllers;
using Common.Base.DTO;
using Common.Base.Responses;
using Microsoft.AspNetCore.Mvc;
using WorkspaceService.Domain.DTOs.WorkspaceRoles;
using WorkspaceService.Domain.Services;

namespace WorkspaceService.Api.Controllers;

public class WorkspaceRoleController : BaseController<WorkspaceRoleController> 
{
    private readonly IWorkspaceRolesService _workspaceRolesService;

    public WorkspaceRoleController(IWorkspaceRolesService workspaceRolesService,
        ILogger<WorkspaceRoleController> logger) : base(logger)
    {
        _workspaceRolesService = workspaceRolesService;
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<RoleDto>> GetAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(GetAsync));
        var result = await _workspaceRolesService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{workspaceId:guid}/list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ListResponse<RoleDto>>> ListAsync(
        Guid workspaceId,
        [FromQuery] ListRequest dto,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(ListAsync));
        var result = await _workspaceRolesService.ListAsync(dto, workspaceId, cancellationToken);
        return Ok(new ListResponse<RoleDto>(result));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateAsync(
        CreateRoleRequest dto,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(CreateAsync));
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
        LogRequest(nameof(UpdateAsync));
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
        LogRequest(nameof(DeleteAsync));
        await _workspaceRolesService.DeleteAsync(id, cancellationToken);
        return Ok();
    }
    
    [HttpPost("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> RestoreAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(RestoreAsync));
        await _workspaceRolesService.RestoreAsync(id, cancellationToken);
        return Ok();
    }
}