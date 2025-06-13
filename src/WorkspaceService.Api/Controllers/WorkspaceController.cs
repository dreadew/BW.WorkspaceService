using Common.AspNetCore.Controllers;
using Common.Base.Context;
using Common.Base.DTO;
using Common.Base.DTO.File;
using Common.Base.Responses;
using Microsoft.AspNetCore.Mvc;
using WorkspaceService.Domain.DTOs.Workspaces;
using WorkspaceService.Domain.DTOs.WorkspaceUsers;
using WorkspaceService.Domain.Services;

namespace WorkspaceService.Api.Controllers;

public class WorkspaceController : BaseController<WorkspaceController> 
{
    private readonly IWorkspaceService _workspaceService;

    public WorkspaceController(IWorkspaceService workspaceService,
        ILogger<WorkspaceController> logger) : base(logger)
    {
        _workspaceService = workspaceService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateAsync(
        [FromBody] CreateWorkspaceRequest dto,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(CreateAsync));
        if (!ModelState.IsValid)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }

        await _workspaceService.CreateAsync(dto, cancellationToken);
        return Ok();
    }

    [HttpPatch("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateAsync(
        [FromBody] UpdateWorkspaceRequest dto,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(UpdateAsync));
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
        Guid id,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(GetAsync));
        var result = await _workspaceService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpGet("list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ListResponse<WorkspaceDto>>> ListAsync(
        [FromQuery] ListRequest dto,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(ListAsync));
        var result = await _workspaceService.ListAsync(dto, cancellationToken);
        return Ok(new ListResponse<WorkspaceDto>(result));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteAsync(
        [FromQuery] DeleteWorkspaceRequest dto,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(DeleteAsync));
        await _workspaceService.DeleteAsync(dto, cancellationToken);
        return Ok();
    }
    
    [HttpPost("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> RestoreAsync(
        [FromQuery] RestoreWorkspaceRequest dto,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(RestoreAsync));
        await _workspaceService.RestoreAsync(dto, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:guid}/{userId:guid}/invite")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> InviteToWorkspaceAsync(
        [FromBody] InviteUserRequest dto,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(InviteToWorkspaceAsync));
        if (!ModelState.IsValid)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }

        await _workspaceService.InviteUserAsync(dto, cancellationToken);
        return Ok();
    }

    [HttpPatch("{id:guid}/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateUserAsync(
        [FromBody] UpdateUserRequest dto,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(UpdateUserAsync));
        if (!ModelState.IsValid)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }

        await _workspaceService.UpdateUserAsync(dto, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:guid}/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteFromWorkspaceAsync(
        [FromRoute] DeleteUserRequest dto,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(DeleteFromWorkspaceAsync));
        await _workspaceService.DeleteUserAsync(dto, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:guid}/picture")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UploadPictureAsync(
        Guid id,
        IFormFile file,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(UploadPictureAsync));
        if (file.Length == 0)
        {
            return BadRequest("Файл пустой");
        }
        
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream, cancellationToken);
        var fileDto = new FileUploadRequest()
        {
            Content = memoryStream.ToArray(),
            FileName = file.FileName,
            ContentType = file.ContentType
        };
        await _workspaceService.UploadPictureAsync(id, fileDto, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:guid}/picture")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeletePictureAsync(
        Guid id,
        [FromQuery] FileDeleteRequest dto,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(DeletePictureAsync));
        await _workspaceService.DeletePictureAsync(id, dto, cancellationToken);
        return Ok();
    }
}