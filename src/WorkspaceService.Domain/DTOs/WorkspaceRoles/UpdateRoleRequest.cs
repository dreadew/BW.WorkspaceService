using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspaceRoles;

public record class UpdateRoleRequest(
    [property: Display(Name="Идентификатор роли")] string Id,
    [property: Display(Name="Название")] string Name,
    [property: Display(Name="Признак актуальность")] bool? IsDeleted);