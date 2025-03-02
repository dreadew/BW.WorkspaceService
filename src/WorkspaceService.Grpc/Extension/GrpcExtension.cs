using Grpc.Net.Client;
using IdentityService.Grpc.Protos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkspaceService.Domain.Services;
using WorkspaceService.Grpc.Services;

namespace WorkspaceService.Grpc.Extension;

public static class GrpcExtension
{
    public static void AddGrpcServices(this IServiceCollection services, IConfiguration configuration)
    {
        var grpcChannel = GrpcChannel.ForAddress(configuration["IdentityServiceUrl"]!);
        services.AddSingleton(grpcChannel);
        services.AddSingleton(provider =>
        {
            var channel = provider.GetRequiredService<GrpcChannel>();
            return new UsersService.UsersServiceClient(channel);
        });
        services.AddSingleton<IIdentityService, GrpcIdentityServiceClient>();
    }
}