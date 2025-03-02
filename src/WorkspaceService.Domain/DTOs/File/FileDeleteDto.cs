namespace WorkspaceService.Domain.DTOs.File;

public record class FileDeleteDto(
    string UserId,
    string FileName);