using DefaultNamespace;

namespace WorkspaceService.Domain.Entities;

public class Workspace : IEntity<Guid>, IAuditable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? PicturePath { get; set; }
    public List<WorkspaceRole> Roles { get; set; } = new();
    public List<WorkspacePosition> Positions { get; set; } = new();
    public List<WorkspaceUser> Users { get; set; } = new();
    public List<WorkspaceDirectory> Directories { get; set; } = new();
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
    public string? ChangedBy { get; set; }
    public string? CreatedBy { get; set; }
}