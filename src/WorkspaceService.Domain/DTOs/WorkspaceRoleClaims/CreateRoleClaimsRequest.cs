namespace WorkspaceService.Domain.DTOs.WorkspaceRoleClaims;

public record class CreateRoleClaimsRequest(string Value,
    string RoleId);