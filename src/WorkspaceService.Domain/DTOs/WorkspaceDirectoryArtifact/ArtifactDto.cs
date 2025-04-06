namespace WorkspaceService.Domain.DTOs.WorkspaceDirectoryArtifact;

public class ArtifactDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}