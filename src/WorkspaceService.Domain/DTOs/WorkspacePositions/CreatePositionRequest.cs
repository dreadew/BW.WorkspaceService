using System.ComponentModel.DataAnnotations;
using Common.Base.DTO.Entity;

namespace WorkspaceService.Domain.DTOs.WorkspacePositions;

public class CreatePositionRequest : BaseDto 
{
    [Display(Name="Название")] 
    public string Name { get; set; }
}