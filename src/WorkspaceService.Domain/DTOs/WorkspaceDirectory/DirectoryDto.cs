﻿using Common.Base.DTO.Entity;
using WorkspaceService.Domain.DTOs.WorkspaceDirectoryArtifact;

namespace WorkspaceService.Domain.DTOs.WorkspaceDirectory;

public class DirectoryDto : BaseSoftDeletableDtoWithName
{
    public DirectoryDto? Parent { get; set; }
    public List<DirectoryDto> Children { get; set; } = new();
    public List<ArtifactDto> Artifacts { get; set; } = new();
}