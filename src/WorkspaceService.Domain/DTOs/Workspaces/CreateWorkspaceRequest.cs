namespace WorkspaceService.Domain.DTOs.Workspaces;

public record class CreateWorkspaceRequest(string UserId,
    string Name);