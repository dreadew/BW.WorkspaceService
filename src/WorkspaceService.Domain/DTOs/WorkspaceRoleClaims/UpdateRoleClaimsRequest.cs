using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspaceRoleClaims;

public record class UpdateRoleClaimsRequest(
    [property: Display(Name="Идентификатор")] string Id,
    [property: Display(Name="Название")] string? Value);