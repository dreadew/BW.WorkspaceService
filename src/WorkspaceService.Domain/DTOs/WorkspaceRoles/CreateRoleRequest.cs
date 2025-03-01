using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspaceRoles;

    public record class CreateRoleRequest(
        [Display(Name="Название")] string Name,
        [Display(Name="Идентификатор рабочего пространства")] string WorkspaceId);