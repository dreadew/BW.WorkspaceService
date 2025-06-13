using Common.Base.DTO.Entity;
using Common.Base.Entities;
using WorkspaceService.Domain.DTOs.WorkspaceDirectory;
using WorkspaceService.Domain.DTOs.WorkspacePositions;
using WorkspaceService.Domain.DTOs.WorkspaceRoles;
using WorkspaceService.Domain.DTOs.WorkspaceUsers;

namespace WorkspaceService.Domain.DTOs.Workspaces;

public class WorkspaceDto : BaseSoftDeletableDtoWithName, ISavable
{
    public string Path { get; set; } = string.Empty;
    // public List<RoleDto> Roles { get; set; }
    // public List<DirectoryDto> Directories { get; set; }
    // public List<PositionDto> Positions { get; set; }
    public List<WorkspaceUserDto> Users { get; set; }
}