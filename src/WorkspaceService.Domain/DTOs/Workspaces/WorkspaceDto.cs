namespace WorkspaceService.Domain.DTOs.Workspaces;

public record class WorkspaceDto(string Id,
    string Name,
    DateTime CreatedAt,
    DateTime? ModifiedAt);