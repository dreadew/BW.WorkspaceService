namespace WorkspaceService.Domain.DTOs.WorkspaceRoleClaims;

public record class UpdateRoleClaimsRequest(string Id,
    string? Value);