using System.ComponentModel.DataAnnotations;
using Common.Base.DTO.Entity;

namespace WorkspaceService.Domain.DTOs.WorkspaceUsers;

public class InviteUserRequest : BaseDto
{
    [Display(Name = "Идентификатор пользователя")]
    public string UserId { get; set; }
}