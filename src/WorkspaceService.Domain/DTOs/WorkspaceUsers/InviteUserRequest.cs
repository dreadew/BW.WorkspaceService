using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspaceUsers;

public class InviteUserRequest
{
    [Display(Name = "Идентификатор рабочего пространства")]
    public string Id { get; set; }
    [Display(Name = "Идентификатор пользователя")]
    public string UserId { get; set; }
    [Display(Name = "Эл. почта")]
    public string Email { get; set; }
}