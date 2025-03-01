namespace WorkspaceService.Domain.DTOs.WorkspaceDirectory;

public record class DirectoryDto(string Id,
    string Name,
    string WorkspaceId,
    DateTime CreatedAt,
    DateTime? ModifiedAt);