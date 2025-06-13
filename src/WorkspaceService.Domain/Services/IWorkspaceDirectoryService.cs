using Common.Base.DTO;
using Common.Base.DTO.Entity;
using Common.Base.DTO.File;
using WorkspaceService.Domain.DTOs.WorkspaceDirectory;

namespace WorkspaceService.Domain.Services;

public interface IWorkspaceDirectoryService
{
    Task CreateAsync(BaseDirectoryRequest dto,
        CancellationToken cancellationToken = default);
    Task UpdateAsync(BaseDirectoryRequest dto,
        CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id,
        CancellationToken cancellationToken = default);
    Task RestoreAsync(Guid id,
        CancellationToken cancellationToken = default);
    Task<DirectoryDto> GetByIdAsync(Guid id,
        CancellationToken cancellationToken = default);
    Task<List<DirectoryDto>> ListAsync(ListRequest dto,
        Guid workspaceId, CancellationToken cancellationToken = default);
    Task UploadArtifactAsync(Guid directoryId, FileUploadRequest dto,
        CancellationToken cancellationToken = default);
    Task DeleteArtifactAsync(Guid directoryId, FileDeleteRequest dto,
        CancellationToken cancellationToken = default);
}