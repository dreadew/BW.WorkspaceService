using Common.AspNetCore.Controllers;
using Common.Base.DTO;
using Microsoft.AspNetCore.Mvc;
using WorkspaceService.Domain.DTOs.WorkspacePositions;
using WorkspaceService.Domain.Entities;
using WorkspaceService.Domain.Services;

namespace WorkspaceService.Api.Controllers;

public class WorkspacePositionController : BaseController<WorkspacePosition> 
{
    private readonly IWorkspacePositionsService _workspacePositionsService;

    public WorkspacePositionController(IWorkspacePositionsService workspacePositionsService,
        ILogger<WorkspacePosition> logger): base(logger)
    {
        _workspacePositionsService = workspacePositionsService;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PositionDto>> GetAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(GetAsync));
        var result = await _workspacePositionsService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{workspaceId:guid}/list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PositionDto>>> ListAsync(    
        Guid workspaceId,
        [FromQuery] ListRequest dto,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(ListAsync));
        var result = await _workspacePositionsService.ListAsync(dto, workspaceId, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateAsync(
        [FromBody] CreatePositionRequest dto,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(CreateAsync));
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
        LogRequest(nameof(UpdateAsync));
        if (!ModelState.IsValid)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }
        
        await _workspacePositionsService.UpdateAsync(dto, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(DeleteAsync));
        await _workspacePositionsService.DeleteAsync(id, cancellationToken);
        return Ok();
    }
    
    [HttpPost("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> RestoreAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(RestoreAsync));
        await _workspacePositionsService.RestoreAsync(id, cancellationToken);
        return Ok();
    }
}