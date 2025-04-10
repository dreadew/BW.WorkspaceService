namespace WorkspaceService.Domain.DTOs.File;

public class FileUploadRequest
{
    public string FromId { get; set; }
    public byte[] Content { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
}