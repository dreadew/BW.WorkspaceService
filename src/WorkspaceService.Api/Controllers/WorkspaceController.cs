using Microsoft.AspNetCore.Mvc;
using WorkspaceService.Domain.DTOs;
using WorkspaceService.Domain.DTOs.File;
using WorkspaceService.Domain.DTOs.Workspaces;
using WorkspaceService.Domain.DTOs.WorkspaceUsers;
using WorkspaceService.Domain.Services;

namespace WorkspaceService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkspaceController : ControllerBase
{
    private readonly IWorkspaceService _workspaceService;
    private readonly ILogger<WorkspaceController> _logger;

    public WorkspaceController(IWorkspaceService workspaceService,
        ILogger<WorkspaceController> logger)
    {
        _workspaceService = workspaceService;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateAsync(
        [FromBody] CreateWorkspaceRequest dto,
        CancellationToken cancellationToken = default)
    {
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
        string id,
        CancellationToken cancellationToken = default)
    {
        var result = await _workspaceService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpGet("list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<WorkspaceDto>>> ListAsync(
        [FromQuery] ListRequest dto,
        CancellationToken cancellationToken = default)
    {
        var userId = HttpContext.Request.Headers["UserId"].FirstOrDefault();
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out _))
        {
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }

        var result = await _workspaceService.ListAsync(dto, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteAsync(
        DeleteWorkspaceRequest dto,
        CancellationToken cancellationToken = default)
    {
        var userId = HttpContext.Request.Headers["UserId"].FirstOrDefault();
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out _))
        {
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }

        await _workspaceService.DeleteAsync(dto, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:guid}/{userId:guid}/invite")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> InviteToWorkspaceAsync(
        [FromBody] InviteUserRequest dto,
        CancellationToken cancellationToken = default)
    {
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
        await _workspaceService.DeleteUserAsync(dto, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:guid}/picture")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UploadPictureAsync(
        string id,
        [FromQuery] string fromId,
        IFormFile file,
        CancellationToken cancellationToken = default)
    {
        if (file.Length == 0)
        {
            return BadRequest("File is empty");
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
        await _workspaceService.UploadPictureAsync(id, fileDto, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:guid}/picture")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeletePictureAsync(
        string id,
        [FromQuery] FileDeleteRequest dto,
        CancellationToken cancellationToken = default)
    {
        await _workspaceService.DeletePictureAsync(id, dto, cancellationToken);
        return Ok();
    }
}