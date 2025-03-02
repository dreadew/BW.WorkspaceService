using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspaceRoles;

    public record class CreateRoleRequest(
        [property: Display(Name="Название")] string Name,
        [property: Display(Name="Идентификатор рабочего пространства")] string WorkspaceId);