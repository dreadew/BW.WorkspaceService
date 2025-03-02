using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspaceRoleClaims;

public record class CreateRoleClaimsRequest(
    [property: Display(Name="Значение")] string Value,
    [property: Display(Name="Идентификатор роли")] string RoleId);