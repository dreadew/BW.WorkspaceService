using System.ComponentModel.DataAnnotations;
using Common.Base.DTO.Entity;

namespace WorkspaceService.Domain.DTOs.WorkspaceRoles;

public class UpdateRoleRequest : BaseRequestDtoWithName
{
    [Display(Name = "Признак актуальность")]
    public bool? IsDeleted { get; set; }
}