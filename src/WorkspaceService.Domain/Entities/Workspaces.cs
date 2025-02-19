using DefaultNamespace;

namespace WorkspaceService.Domain.Entities;

public class Workspaces : IEntity<string>, IAuditable
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string PictureUrl { get; set; } = string.Empty;
    public List<WorkspaceRoles> Roles { get; set; } = new List<WorkspaceRoles>();
    public List<WorkspacePositions> Positions { get; set; } = new List<WorkspacePositions>();
    public List<WorkspaceUsers> Users { get; set; } = new List<WorkspaceUsers>();
    public List<WorkspaceDirectory> Directories { get; set; } = new List<WorkspaceDirectory>();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
}