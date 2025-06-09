using Common.Base.DTO.Entity;
using WorkspaceService.Domain.DTOs.WorkspaceDirectory;
using WorkspaceService.Domain.DTOs.WorkspacePositions;
using WorkspaceService.Domain.DTOs.WorkspaceRoles;
using WorkspaceService.Domain.DTOs.WorkspaceUsers;

namespace WorkspaceService.Domain.DTOs.Workspaces;

public class WorkspaceWithAdditionalInfoDto : BaseSoftDeletableDto
{
    public string Name { get; set; }
    public List<DirectoryDto> Directories { get; set; }
    public List<PositionDto> Positions { get; set; }
    public List<RoleDto> Roles { get; set; }
    public List<WorkspaceUserDto> Users { get; set; }
}