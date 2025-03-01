namespace WorkspaceService.Domain.DTOs.WorkspaceRoles;

    public record class CreateRoleRequest(string Name,
        string WorkspaceId);