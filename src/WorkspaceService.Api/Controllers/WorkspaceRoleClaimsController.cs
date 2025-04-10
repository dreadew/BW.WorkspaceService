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
    public async Task<ActionResult<RoleClaimsDto>> GetAsync(string id,
        CancellationToken cancellationToken = default)
    {
        var result = await _workspaceRoleClaimsService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpGet("list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetListAsync(
        [FromQuery] ListRequest dto,
        CancellationToken cancellationToken = default)
    {
        var roleId = HttpContext.Request.Headers["RoleId"].FirstOrDefault();
        if (string.IsNullOrEmpty(roleId) || !Guid.TryParse(roleId, out _))
        {
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }
        
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

    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateAsync(
        UpdateRoleClaimsRequest dto,
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
    public async Task<ActionResult> DeleteAsync(string id,
        CancellationToken cancellationToken = default)
    {
        await _workspaceRoleClaimsService.DeleteAsync(id, cancellationToken);
        return Ok();
    }
}