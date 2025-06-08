using Common.AspNetCore.Extensions;
using Common.AspNetCore.Middlewares;
using WorkspaceService.Application.Extension;
using WorkspaceService.Grpc.Extension;
using WorkspaceService.Infrastructure.Data;
using WorkspaceService.Infrastructure.Extension;

WebApplication.CreateBuilder(args)
    .ConfigureMicroservice(builder =>
    {
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddApplication();
        builder.Services.AddGrpcServices(builder.Configuration);
    })
    .Build()
    .ApplyMigrations<ApplicationDbContext>()
    .ConfigureAndRunMicroservice(app =>
    {
        app.UseMiddleware<GrpcAuthMiddleware>();
    }, app =>
    {
        app.MapGrpcService<WorkspaceService.Grpc.Services.WorkspaceService>();   
    });