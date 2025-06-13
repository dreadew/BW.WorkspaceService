using Common.AspNetCore.Controllers;
using Common.Base.Constants;
using Common.Base.Context;
using Common.Base.DTO;
using Common.Base.DTO.Entity;
using Common.Base.DTO.File;
using Common.Base.Services;
using Microsoft.AspNetCore.Mvc;
using WorkspaceService.Domain.DTOs.WorkspaceDirectory;
using WorkspaceService.Domain.Entities;
using WorkspaceService.Domain.Services;

namespace WorkspaceService.Api.Controllers;

public class WorkspaceDirectoryController : BaseController<WorkspaceDirectoryController> 
{
    private readonly IBaseResourceDirectoryService<Workspace, WorkspaceDirectory, WorkspaceDirectoryArtifact, WorkspaceUser> _directoryService;

    public WorkspaceDirectoryController(
        IBaseResourceDirectoryService<Workspace, WorkspaceDirectory, WorkspaceDirectoryArtifact, WorkspaceUser> directoryService,
        ILogger<WorkspaceDirectoryController> logger) : base(logger)
    {
        _directoryService = directoryService;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<DirectoryDto>> GetAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(GetAsync));
        var result = await _directoryService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{workspaceId:guid}/list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<DirectoryDto>>> ListAsync(
        Guid workspaceId,
        [FromQuery] ListRequest dto,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(ListAsync));
        var result = await _directoryService.ListAsync(dto, workspaceId,
            cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateAsync([FromBody] BaseDirectoryRequest dto,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(CreateAsync));
        if (!ModelState.IsValid)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }
        
        await _directoryService.CreateAsync(dto, cancellationToken);
        return Ok();
    }

    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateAsync([FromBody] BaseDirectoryRequest dto,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(UpdateAsync));
        if (!ModelState.IsValid)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }
        
        await _directoryService.UpdateAsync(dto, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(DeleteAsync));
        await _directoryService.DeleteAsync(id, cancellationToken);
        return Ok();
    }
    
    [HttpPost("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> RestoreAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(RestoreAsync));
        await _directoryService.RestoreAsync(id, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:guid}/artifact")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UploadArtifactAsync(
        Guid id,
        IFormFile file,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(UploadArtifactAsync));
        if (file.Length == 0)
        {
            return BadRequest(ExceptionResourceKeys.FileIsEmpty);
        }
        
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream, cancellationToken);
        var fileDto = new FileUploadRequest()
        {
            Content = memoryStream.ToArray(),
            FileName = file.FileName,
            ContentType = file.ContentType
        };

        await _directoryService.UploadArtifactAsync(id, fileDto, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:guid}/artifact")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteArtifactAsync(
        Guid id,
        [FromQuery] FileDeleteRequest dto,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(DeleteArtifactAsync));
        await _directoryService.DeleteArtifactAsync(id, dto, cancellationToken);
        return Ok();
    }
}