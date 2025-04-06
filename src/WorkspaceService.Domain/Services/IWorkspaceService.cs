using WorkspaceService.Domain.DTOs;
using WorkspaceService.Domain.DTOs.File;
using WorkspaceService.Domain.DTOs.Workspaces;
using WorkspaceService.Domain.DTOs.WorkspaceUsers;

namespace WorkspaceService.Domain.Services;

public interface IWorkspaceService
{
    Task CreateAsync(CreateWorkspaceRequest dto,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(UpdateWorkspaceRequest dto,
        CancellationToken cancellationToken = default);

    Task<WorkspaceDto> GetByIdAsync(string id,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<WorkspaceDto>> ListAsync(ListRequest dto,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(DeleteWorkspaceRequest dto,
        CancellationToken cancellationToken = default);

    Task InviteUserAsync(InviteUserRequest dto,
        CancellationToken cancellationToken = default);

    Task UpdateUserAsync(UpdateUserRequest dto,
        CancellationToken cancellationToken = default);

    Task DeleteUserAsync(DeleteUserRequest dto,
        CancellationToken cancellationToken = default);

    Task UploadPictureAsync(string workspaceId,
        FileUploadRequest dto,
        CancellationToken cancellationToken = default);

    Task DeletePictureAsync(string workspaceId,
        FileDeleteRequest dto,
        CancellationToken cancellationToken = default);
}