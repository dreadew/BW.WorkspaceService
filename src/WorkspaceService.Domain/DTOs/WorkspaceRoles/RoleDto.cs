namespace WorkspaceService.Domain.DTOs.WorkspaceRoles;

public record class RoleDto(string Id,
    string Name,
    DateTime CreatedAt,
    DateTime? ModifiedAt);