using Common.Base.DTO.Entity;

namespace WorkspaceService.Domain.DTOs.WorkspaceDirectoryArtifact;

public class ArtifactDto : BaseDto
{
    public string Name { get; set; }
    public string Path { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}