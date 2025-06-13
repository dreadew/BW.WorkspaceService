using System.ComponentModel.DataAnnotations;
using Common.Base.DTO.Entity;

namespace WorkspaceService.Domain.DTOs.Workspaces;

public class UpdateWorkspaceRequest : BaseRequestDtoWithName
{
    [Display(Name = "Признак актуальность")]
    public bool? IsDeleted { get; set; }
}