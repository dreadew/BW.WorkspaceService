using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspaceUsers;

public record class InviteUserRequest(
    [property: Display(Name="Идентификатор рабочего пространства")] string Id, 
    [property: Display(Name="Идентификатор пользователя")] string UserId,
    [property: Display(Name="Эл. почта")] string Email);