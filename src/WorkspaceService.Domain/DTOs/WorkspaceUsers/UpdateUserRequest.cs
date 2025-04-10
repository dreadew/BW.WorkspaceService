using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspaceUsers;

public class UpdateUserRequest
{
    [Display(Name = "Идентификатор пользователя")]
    public string FromId { get; set; }
    [Display(Name = "Идентификатор рабочего пространства")]
    public string WorkspaceId { get; set; }
    [Display(Name = "Идентификатор пользователя")]
    public string UserId { get; set; }
    [Display(Name = "Идентификатор роли")]
    public string? RoleId { get; set; }
    [Display(Name = "Идентификатор должности")]
    public string? PositionId { get; set; }
}