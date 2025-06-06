using WorkspaceService.Domain.DTOs.WorkspaceDirectoryArtifact;

namespace WorkspaceService.Domain.DTOs.WorkspaceDirectory;

public class DirectoryDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public DirectoryDto? Parent { get; set; }
    public List<DirectoryDto> Children { get; set; } = new();
    public List<ArtifactDto> Artifacts { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}