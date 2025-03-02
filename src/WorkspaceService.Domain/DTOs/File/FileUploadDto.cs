namespace WorkspaceService.Domain.DTOs.File;

public record class FileUploadDto(
    string UserId,
    byte[] Content,
    string FileName,
    string ContentType);