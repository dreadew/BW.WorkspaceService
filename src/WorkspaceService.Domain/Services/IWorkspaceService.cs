using Common.Base.DTO;
using Common.Base.DTO.File;
using WorkspaceService.Domain.DTOs.Workspaces;
using WorkspaceService.Domain.DTOs.WorkspaceUsers;

namespace WorkspaceService.Domain.Services;

public interface IWorkspaceService
{
    Task CreateAsync(CreateWorkspaceRequest dto,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(UpdateWorkspaceRequest dto,
        CancellationToken cancellationToken = default);

    Task<WorkspaceDto> GetByIdAsync(Guid id,
        CancellationToken cancellationToken = default);

    Task<(List<WorkspaceDto>, long)> ListAsync(ListRequest dto,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(DeleteWorkspaceRequest dto,
        CancellationToken cancellationToken = default);

    Task RestoreAsync(RestoreWorkspaceRequest dto,
        CancellationToken cancellationToken = default);

    Task InviteUserAsync(InviteUserRequest dto,
        CancellationToken cancellationToken = default);

    Task UpdateUserAsync(UpdateUserRequest dto,
        CancellationToken cancellationToken = default);

    Task DeleteUserAsync(DeleteUserRequest dto,
        CancellationToken cancellationToken = default);

    Task UploadPictureAsync(Guid workspaceId,
        FileUploadRequest dto,
        CancellationToken cancellationToken = default);

    Task DeletePictureAsync(Guid workspaceId,
        FileDeleteRequest dto,
        CancellationToken cancellationToken = default);
}