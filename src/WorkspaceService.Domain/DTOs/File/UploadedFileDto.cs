namespace WorkspaceService.Domain.DTOs.File;

public record class UploadedFileResponse(
    string FilePath,
    string Url);