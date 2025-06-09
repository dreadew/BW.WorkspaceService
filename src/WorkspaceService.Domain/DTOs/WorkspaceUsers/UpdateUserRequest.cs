using System.ComponentModel.DataAnnotations;
using Common.Base.DTO.Entity;

namespace WorkspaceService.Domain.DTOs.WorkspaceUsers;

public class UpdateUserRequest : BaseDto
{
    [Display(Name = "Идентификатор пользователя")]
    public string UserId { get; set; }
    [Display(Name = "Идентификатор роли")]
    public string? RoleId { get; set; }
    [Display(Name = "Идентификатор должности")]
    public string? PositionId { get; set; }
}