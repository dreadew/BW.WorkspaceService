using DefaultNamespace;

namespace WorkspaceService.Domain.Entities;

public class Workspace : IEntity<string>, IAuditable
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? PicturePath { get; set; }
    public List<WorkspaceRole> Roles { get; set; } = new List<WorkspaceRole>();
    public List<WorkspacePosition> Positions { get; set; } = new List<WorkspacePosition>();
    public List<WorkspaceUser> Users { get; set; } = new List<WorkspaceUser>();
    public List<WorkspaceDirectory> Directories { get; set; } = new List<WorkspaceDirectory>();
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
    public string? ChangedBy { get; set; }
    public string? CreatedBy { get; set; }
}