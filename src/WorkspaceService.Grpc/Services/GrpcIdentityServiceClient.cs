using IdentityService.Grpc.Protos;
using Microsoft.Extensions.Logging;
using WorkspaceService.Domain.Services;

namespace WorkspaceService.Grpc.Services;

public class GrpcIdentityServiceClient : IIdentityService
{
    private readonly UsersService.UsersServiceClient _client;
    private readonly ILogger<GrpcIdentityServiceClient> _logger;

    public GrpcIdentityServiceClient(UsersService.UsersServiceClient client,
        ILogger<GrpcIdentityServiceClient> logger)
    {
        _client = client;
        _logger = logger;
    }
    
    public async Task<bool> VerifyAsync(string accessToken)
    {
        try
        {
            var request = new VerifyRequest { AccessToken = accessToken };
            var response = await _client.VerifyAsync(request);
            return response.IsValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при вызове gRPC Verify");
            return false;
        }
    }
}