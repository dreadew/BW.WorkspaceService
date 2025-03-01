using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspaceRoles;

public record class UpdateRoleRequest(
    [Display(Name="Идентификатор роли")] string Id,
    [Display(Name="Название")] string Name,
    [Display(Name="Идентификатор рабочего пространства")] string WorkspaceId);