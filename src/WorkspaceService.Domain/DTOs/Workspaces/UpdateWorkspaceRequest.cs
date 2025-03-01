namespace WorkspaceService.Domain.DTOs.Workspaces;

public record class UpdateWorkspaceRequest(string Id,
    string Name);