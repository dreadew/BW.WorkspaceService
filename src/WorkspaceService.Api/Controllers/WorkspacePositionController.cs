using Microsoft.AspNetCore.Mvc;
using WorkspaceService.Domain.DTOs;
using WorkspaceService.Domain.DTOs.WorkspacePositions;
using WorkspaceService.Domain.DTOs.Workspaces;
using WorkspaceService.Domain.Services;

namespace WorkspaceService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkspacePositionController : ControllerBase
{
    private readonly IWorkspacePositionsService _workspacePositionsService;
    private readonly ILogger<WorkspaceRoleClaimsController> _logger;

    public WorkspacePositionController(IWorkspacePositionsService workspacePositionsService,
        ILogger<WorkspaceRoleClaimsController> logger)
    {
        _workspacePositionsService = workspacePositionsService;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PositionDto>> GetAsync(string id,
        CancellationToken cancellationToken = default)
    {
        var result = await _workspacePositionsService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpGet("list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PositionDto>>> ListAsync(
        [FromQuery] ListRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspaceId = HttpContext.Request.Headers["WorkspaceId"].FirstOrDefault();
        if (string.IsNullOrEmpty(workspaceId) || !Guid.TryParse(workspaceId, out _))
        {
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }
        
        var result = await _workspacePositionsService.ListAsync(dto, workspaceId, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateAsync(
        [FromBody] CreatePositionRequest dto,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }
        
        await _workspacePositionsService.CreateAsync(dto, cancellationToken);
        return Ok();
    }

    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateAsync(
        [FromBody] UpdatePositionRequest dto,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }
        
        await _workspacePositionsService.UpdateAsync(dto, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteAsync(string id,
        CancellationToken cancellationToken = default)
    {
        await _workspacePositionsService.DeleteAsync(id, cancellationToken);
        return Ok();
    }
}