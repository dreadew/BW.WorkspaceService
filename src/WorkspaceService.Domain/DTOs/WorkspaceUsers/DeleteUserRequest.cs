using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspaceUsers;

public class DeleteUserRequest
{
    [Display(Name = "Идентификатор пользователя")]
    public string FromId { get; set; }
    [Display(Name = "Идентификатор рабочего пространства")]
    public string WorkspaceId { get; set; }
    [Display(Name = "Идентификатор пользователя")]
    public string UserId { get; set; }
}