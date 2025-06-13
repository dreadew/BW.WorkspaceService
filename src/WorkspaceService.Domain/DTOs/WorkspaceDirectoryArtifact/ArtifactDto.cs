using Common.Base.DTO.Entity;
using Common.Base.Entities;

namespace WorkspaceService.Domain.DTOs.WorkspaceDirectoryArtifact;

public class ArtifactDto : BaseDtoWithName, ISavable
{
    public string Path { get; set; }
}