using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspaceUsers;

public record class UpdateUserRequest(
    [property: Display(Name="Идентификатор пользователя")] string FromId,
    [property: Display(Name="Идентификатор рабочего пространства")] string WorkspaceId,
    [property: Display(Name="Идентификатор пользователя")] string UserId, 
    [property: Display(Name="Идентификатор роли")]string? RoleId, 
    [property: Display(Name="Идентификатор должности")] string? PositionId);