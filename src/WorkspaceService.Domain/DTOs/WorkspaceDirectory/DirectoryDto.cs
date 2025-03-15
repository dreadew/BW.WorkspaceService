using WorkspaceService.Domain.DTOs.WorkspaceDirectoryArtifact;

namespace WorkspaceService.Domain.DTOs.WorkspaceDirectory;

public record class DirectoryDto(string Id,
    string Name,
    List<DirectoryDto> Children,
    List<ArtifactDto>  Artifacts,
    DateTime CreatedAt,
    DateTime? ModifiedAt);