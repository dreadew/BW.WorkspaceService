namespace WorkspaceService.Domain.Options;

public class S3Options
{
    public string Endpoint { get; set; }
    public string Bucket  { get; set; }
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string Region  { get; set; }
}