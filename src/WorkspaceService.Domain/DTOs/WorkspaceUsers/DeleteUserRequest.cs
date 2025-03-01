using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspaceUsers;

public record class DeleteUserRequest(
    [Display(Name="Идентификатор рабочего пространства")] string WorkspaceId,
    [Display(Name="Идентификатор пользователя")] string UserId);