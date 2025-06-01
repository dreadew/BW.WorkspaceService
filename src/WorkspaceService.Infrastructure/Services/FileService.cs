using System.Text;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WorkspaceService.Domain.Constants;
using WorkspaceService.Domain.DTOs.File;
using WorkspaceService.Domain.Interfaces;
using WorkspaceService.Domain.Options;

namespace WorkspaceService.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly IAmazonS3 _s3Client;
    private readonly IOptions<S3Options> _options;
    private readonly ILogger<FileService> _logger;

    public FileService(
        IOptions<S3Options> options,
        ILogger<FileService> logger)
    {
        _logger = logger;
        _options = options;
        
        var config = new AmazonS3Config
        {
            ServiceURL = _options.Value.Endpoint,
            ForcePathStyle = true,
            UseHttp = true
        };
        
        _s3Client = new AmazonS3Client(_options.Value.AccessKey, _options.Value.SecretKey, config);
    }
    
    public async Task<UploadedFileResponse> UploadFileAsync(
        FileUploadDto dto,
        CancellationToken cancellationToken = default)
    {
        await _s3Client.EnsureBucketExistsAsync(_options.Value.Bucket);
        
        bool found = await AmazonS3Util
            .DoesS3BucketExistV2Async(_s3Client,
                _options.Value.Bucket);
        if (!found)
        {
            var putBucketRequest = new PutBucketRequest
            {
                BucketName = _options.Value.Bucket,
                UseClientRegion = true
            };
            await _s3Client.PutBucketAsync(putBucketRequest, 
                cancellationToken);
        }
        
        var fileExt = Path.GetExtension(dto.FileName);
        var expectedCount = 36 + fileExt.Length + (dto.Paths?.Select(x => x.Length).Sum
            () ?? 0);
        var sb = new StringBuilder(expectedCount);
        if (dto.Paths != null)
        {
            foreach (var path in dto.Paths)
            {
                sb.Append($"{path}/");
            }
        }
        sb.Append($"{Guid.NewGuid().ToString()}{fileExt}");
        var objectName = sb.ToString();

        using var stream = new MemoryStream(dto.Content);
        var putObjectRequest = new PutObjectRequest
        {
            BucketName = _options.Value.Bucket,
            Key = objectName,
            InputStream = stream,
            ContentType = dto.ContentType
        };
        
        await _s3Client
            .PutObjectAsync(putObjectRequest, 
                cancellationToken);

        var url = $"{_options.Value.Bucket}/{objectName}";
        
        return new UploadedFileResponse()
        {
            FilePath = objectName,
            Url = url
        };
    }

    public async Task DeleteFileAsync(
        FileDeleteDto dto,
        CancellationToken cancellationToken = default)
    {
        await _s3Client.EnsureBucketExistsAsync(_options.Value.Bucket);
        
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _options.Value.Bucket,
            Key = dto.FileName
        };
        
        await _s3Client
            .DeleteObjectAsync(deleteRequest,
                cancellationToken);
    }
}