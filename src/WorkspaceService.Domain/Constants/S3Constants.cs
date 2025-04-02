namespace WorkspaceService.Domain.Constants;

public static class S3Constants
{
    public const string Section = "S3";
    public const string Endpoint = $"{Section}:Endpoint";
    public const string Bucket = $"{Section}:Bucket";
    public const string AccessKey = $"{Section}:AccessKey";
    public const string SecretKey = $"{Section}:SecretKey";
    public const string Region = $"{Section}:Region";
}