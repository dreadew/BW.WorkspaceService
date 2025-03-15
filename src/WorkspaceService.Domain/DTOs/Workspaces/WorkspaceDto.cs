using WorkspaceService.Domain.DTOs.WorkspaceDirectory;
using WorkspaceService.Domain.DTOs.WorkspacePositions;
using WorkspaceService.Domain.DTOs.WorkspaceRoles;
using WorkspaceService.Domain.DTOs.WorkspaceUsers;

namespace WorkspaceService.Domain.DTOs.Workspaces;

public record class WorkspaceDto(
    string Id,
    string Name,
    string PictureUrl,
    List<RoleDto> Roles,
    List<DirectoryDto> Directories,
    List<PositionDto> Positions,
    List<WorkspaceUserDto> Users,
    DateTime CreatedAt,
    DateTime? ModifiedAt);