using System.ComponentModel.DataAnnotations;
using Common.Base.DTO.Entity;

namespace WorkspaceService.Domain.DTOs.WorkspaceUsers;

public class DeleteUserRequest : BaseDto
{
    [Display(Name = "Идентификатор пользователя")]
    public string UserId { get; set; }
}