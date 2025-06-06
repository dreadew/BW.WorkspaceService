using Microsoft.AspNetCore.Mvc;
using WorkspaceService.Api.Controllers.Base;
using WorkspaceService.Domain.Constants;
using WorkspaceService.Domain.DTOs;
using WorkspaceService.Domain.DTOs.File;
using WorkspaceService.Domain.DTOs.WorkspaceDirectory;
using WorkspaceService.Domain.Services;

namespace WorkspaceService.Api.Controllers;

public class WorkspaceDirectoryController : BaseController<WorkspaceDirectoryController> 
{
    private readonly IWorkspaceDirectoryService _workspaceDirectoryService;

    public WorkspaceDirectoryController(IWorkspaceDirectoryService workspaceDirectoryService,
        ILogger<WorkspaceDirectoryController> logger) : base(logger)
    {
        _workspaceDirectoryService = workspaceDirectoryService;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<DirectoryDto>> GetAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(GetAsync));
        var result = await _workspaceDirectoryService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{workspaceId:guid}/list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DirectoryDto>>> ListAsync(
        Guid workspaceId,
        [FromQuery] ListRequest dto,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(ListAsync));
        var result = await _workspaceDirectoryService.ListAsync(dto, workspaceId,
            cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateAsync([FromBody] CreateDirectoryRequest dto,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(CreateAsync));
        if (!ModelState.IsValid)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }
        
        await _workspaceDirectoryService.CreateAsync(dto, cancellationToken);
        return Ok();
    }

    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateAsync([FromBody] UpdateDirectoryRequest dto,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(UpdateAsync));
        if (!ModelState.IsValid)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }
        
        await _workspaceDirectoryService.UpdateAsync(dto, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(DeleteAsync));
        await _workspaceDirectoryService.DeleteAsync(id, cancellationToken);
        return Ok();
    }
    
    [HttpPost("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> RestoreAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        LogRequest(nameof(RestoreAsync));
        await _workspaceDirectoryService.RestoreAsync(id, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:guid}/artifact")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UploadArtifactAsync(
        Guid id,
        [FromQuery] string fromId,
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
            FromId = fromId, 
            Content = memoryStream.ToArray(),
            FileName = file.FileName,
            ContentType = file.ContentType
        };
        
        await _workspaceDirectoryService.UploadArtifactAsync(id, fileDto, cancellationToken);
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
        await _workspaceDirectoryService.DeleteArtifactAsync(id, dto, cancellationToken);
        return Ok();
    }
}