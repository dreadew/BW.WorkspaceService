using WorkspaceService.Domain.DTOs;
using WorkspaceService.Domain.DTOs.WorkspaceDirectory;

namespace WorkspaceService.Domain.Services;

public interface IWorkspaceDirectoryService
{
    Task CreateAsync(CreateDirectoryRequest dto,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(UpdateDirectoryRequest dto,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string id,
        CancellationToken cancellationToken = default);

    Task<DirectoryDto> GetByIdAsync(string id,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<DirectoryDto>> ListAsync(ListRequest dto,
        string workspaceId, CancellationToken cancellationToken = default);
}