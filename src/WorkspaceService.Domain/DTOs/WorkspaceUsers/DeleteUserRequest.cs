using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspaceUsers;

public record class DeleteUserRequest(
    [property: Display(Name="Идентификатор пользователя")] string FromId,
    [property: Display(Name="Идентификатор рабочего пространства")] string WorkspaceId,
    [property: Display(Name="Идентификатор пользователя")] string UserId);