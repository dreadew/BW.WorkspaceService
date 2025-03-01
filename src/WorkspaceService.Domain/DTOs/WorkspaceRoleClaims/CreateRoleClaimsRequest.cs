using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspaceRoleClaims;

public record class CreateRoleClaimsRequest(
    [Display(Name="Значение")] string Value,
    [Display(Name="Идентификатор роли")] string RoleId);