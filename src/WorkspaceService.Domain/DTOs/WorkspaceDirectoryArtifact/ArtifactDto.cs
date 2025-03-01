namespace WorkspaceService.Domain.DTOs.WorkspaceDirectoryArtifact;

public record class ArtifactDto(string Id,
    string Name,
    DateTime CreatedAt,
    DateTime? ModifiedAt);