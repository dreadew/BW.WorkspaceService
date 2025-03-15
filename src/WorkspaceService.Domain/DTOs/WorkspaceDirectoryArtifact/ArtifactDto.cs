namespace WorkspaceService.Domain.DTOs.WorkspaceDirectoryArtifact;

public record class ArtifactDto(string Id,
    string Name,
    string Url,
    DateTime CreatedAt,
    DateTime? ModifiedAt);