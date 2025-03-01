using WorkspaceService.Domain.DTOs.WorkspaceDirectory;
using WorkspaceService.Domain.DTOs.WorkspacePositions;
using WorkspaceService.Domain.DTOs.WorkspaceRoles;
using WorkspaceService.Domain.DTOs.WorkspaceUsers;

namespace WorkspaceService.Domain.DTOs.Workspaces;

public record class WorkspaceWithAdditionalInfoDto(string Id,
    string Name,
    List<DirectoryDto> Directories,
    List<PositionDto> Positions,
    List<RoleDto> Roles,
    List<WorkspaceUserDto> Users,
    DateTime CreatedAt,
    DateTime? ModifiedAt);