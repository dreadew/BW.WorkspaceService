using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Microsoft.Extensions.Logging;
using WorkspaceService.Domain.Constants;
using WorkspaceService.Domain.DTOs.File;
using WorkspaceService.Domain.Interfaces;

namespace WorkspaceService.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;
    private readonly string _s3Endpoint;
    private readonly ILogger<FileService> _logger;
    
    public FileService(
        ISecretsProvider secretsProvider,
        ILogger<FileService> logger)
    {
        _logger = logger;
        
        _bucketName = secretsProvider
            .GetSecret(
                S3Constants.Bucket,
                "dev");
        _s3Endpoint = secretsProvider
            .GetSecret(
                S3Constants.Endpoint,
                "dev");
        var accessKey = secretsProvider
            .GetSecret(
                S3Constants.AccessKey,
                "dev");
        var secretKey = secretsProvider
            .GetSecret(
                S3Constants.SecretKey,
                "dev");
        var region = secretsProvider
            .GetSecret(
                S3Constants.Region,
                "dev");

        var config = new AmazonS3Config
        {
            ServiceURL = _s3Endpoint,
            ForcePathStyle = true,
            UseHttp = true
        };
        if (!string.IsNullOrEmpty(region))
        {
            //config.RegionEndpoint = RegionEndpoint.GetBySystemName(region);
        }
        
        _s3Client = new AmazonS3Client(accessKey, secretKey, config);
    }
    
    public async Task<UploadedFileResponse> UploadProfilePhotoAsync(
        FileUploadDto dto,
        CancellationToken cancellationToken = default)
    {
        bool found = await AmazonS3Util
            .DoesS3BucketExistV2Async(_s3Client,
                _bucketName);
        if (!found)
        {
            var putBucketRequest = new PutBucketRequest
            {
                BucketName = _bucketName,
                UseClientRegion = true
            };
            await _s3Client.PutBucketAsync(putBucketRequest, 
                cancellationToken);
        }
        
        var fileExt = Path.GetExtension(dto.FileName);
        string objectName = $"{Guid.NewGuid().ToString()}{fileExt}";

        using var stream = new MemoryStream(dto.Content);
        var putObjectRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = objectName,
            InputStream = stream,
            ContentType = dto.ContentType
        };
        
        await _s3Client
            .PutObjectAsync(putObjectRequest, 
                cancellationToken);

        string url = $"{_s3Endpoint}/{_bucketName}/{objectName}";
        
        return new UploadedFileResponse(objectName, url);
    }

    public async Task DeleteProfilePhotoAsync(
        FileDeleteDto dto,
        CancellationToken cancellationToken = default)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = dto.FileName
        };
        
        await _s3Client
            .DeleteObjectAsync(deleteRequest,
                cancellationToken);
    }
}