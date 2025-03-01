using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspaceUsers;

public record class UpdateUserRequest(
    [Display(Name="Идентификатор рабочего пространства")] string WorkspaceId,
    [Display(Name="Идентификатор пользователя")] string UserId, 
    [Display(Name="Идентификатор роли")]string? RoleId, 
    [Display(Name="Идентификатор должности")] string? PositionId);