using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspaceUsers;

public record class InviteUserRequest(
    [Display(Name="Идентификатор рабочего пространства")] string Id, 
    [Display(Name="Идентификатор пользователя")] string UserId,
    [Display(Name="Эл. почта")] string Email);