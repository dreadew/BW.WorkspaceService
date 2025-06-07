using Common.Base.DTO.Grpc;
using Common.Base.Services;
using IdentityService.Grpc.Protos;
using Microsoft.Extensions.Logging;

namespace WorkspaceService.Grpc.Clients;

public class IdentityServiceClient : IIdentityServiceClient
{
    private readonly UsersService.UsersServiceClient _client;
    private readonly ILogger<IdentityServiceClient> _logger;

    public IdentityServiceClient(UsersService.UsersServiceClient client,
        ILogger<IdentityServiceClient> logger)
    {
        _client = client;
        _logger = logger;
    }
    
    public async Task<(bool, string?)> VerifyAsync(string accessToken, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new VerifyRequest { AccessToken = accessToken };
            var response = await _client.VerifyAsync(request);
            return (response.IsValid, response.UserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при вызове gRPC Verify");
            return (false, null);
        }
    }

    public async Task<UserDto?> GetByIdAsync(string id, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new GetByIdRequest() { Id = id };
            var user = (await _client.GetByIdAsync(request)).User;

            var userDto = new UserDto()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                PhotoPath = user.PhotoPath,
                CreatedAt = user.CreatedAt.ToDateTime(),
                UpdatedAt = user.UpdatedAt?.ToDateTime() 
            };
            
            return userDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при вызове gRPC GetByIdAsync");
            return null;
        }
    }

    public async Task<List<UserDto>> GetFromArrayAsync(List<string> userIds,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new GetFromIdArrayRequest() { Ids = { userIds } };
            var response = (await _client.GetFromIdArrayAsync(request)).Users;
            var users = new List<UserDto>();

            foreach (var user in response)
            {
                var userDto = new UserDto()
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    PhotoPath = user.PhotoPath,
                    CreatedAt = user.CreatedAt.ToDateTime(),
                    UpdatedAt = user.UpdatedAt?.ToDateTime()
                };
                
                users.Add(userDto);
            }
            
            return users;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при вызове gRPC GetFromArrayAsync");
            return null;
        }
    }
}