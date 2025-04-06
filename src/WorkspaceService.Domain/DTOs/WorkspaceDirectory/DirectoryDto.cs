using WorkspaceService.Domain.DTOs.WorkspaceDirectoryArtifact;

namespace WorkspaceService.Domain.DTOs.WorkspaceDirectory;

public class DirectoryDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<DirectoryDto> Children { get; set; }
    public List<ArtifactDto> Artifacts { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}