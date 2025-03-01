using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspaceRoleClaims;

public record class UpdateRoleClaimsRequest(
    [Display(Name="Идентификатор")] string Id,
    [Display(Name="Название")] string? Value);