namespace WorkspaceService.Domain.DTOs.WorkspaceRoles;

public record class UpdateRoleRequest(string Id,
    string Name,
    string WorkspaceId);