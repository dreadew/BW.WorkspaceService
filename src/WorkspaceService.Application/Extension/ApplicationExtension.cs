

using System.Globalization;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using WorkspaceService.Application.Mapping;
using WorkspaceService.Application.Services;
using WorkspaceService.Application.Validators.WorkspaceDirectory;
using WorkspaceService.Application.Validators.WorkspacePositions;
using WorkspaceService.Application.Validators.WorkspaceRoles;
using WorkspaceService.Application.Validators.Workspaces;
using WorkspaceService.Application.Validators.WorkspaceUsers;
using WorkspaceService.Domain.DTOs.WorkspaceDirectory;
using WorkspaceService.Domain.DTOs.WorkspacePositions;
using WorkspaceService.Domain.DTOs.WorkspaceRoles;
using WorkspaceService.Domain.DTOs.Workspaces;
using WorkspaceService.Domain.DTOs.WorkspaceUsers;
using WorkspaceService.Domain.Services;
using WorkspaceService.Domain.Utils;

namespace WorkspaceService.Application.Extension;

public static class ApplicationExtension
{
    /// <summary>
    /// Внедрение зависимостей слоя Application
    /// </summary>
    /// <param name="services"></param>
    public static void AddApplication(this IServiceCollection services)
    {
        InitMappers(services);
        InitValidators(services);
        InitServices(services);
    }

    /// <summary>
    /// Внедрение зависимостей маппинга
    /// </summary>
    /// <param name="services"></param>
    private static void InitMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(WorkspacesProfile));
        services.AddAutoMapper(typeof(WorkspacePositionsProfile));
        services.AddAutoMapper(typeof(WorkspaceRolesProfile));
        services.AddAutoMapper(typeof(WorkspaceDirectoryProfile));
    }

    /// <summary>
    /// Внедрение зависимостей валидаций
    /// </summary>
    /// <param name="services"></param>
    private static void InitValidators(this IServiceCollection services)
    {
        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("ru");
        ValidatorOptions.Global.DisplayNameResolver = (type, memberInfo, expression) =>
            memberInfo != null ? ValidationHelper.GetDisplayName(type, memberInfo.Name) : null;
        services.AddScoped<IValidator<CreateDirectoryRequest>, CreateDirectoryRequestValidator>();
        services.AddScoped<IValidator<UpdateDirectoryRequest>, UpdateDirectoryRequestValidator>();
        services.AddScoped<IValidator<CreatePositionRequest>, CreatePositionRequestValidator>();
        services.AddScoped<IValidator<UpdatePositionRequest>, UpdatePositionRequestValidator>();
        services.AddScoped<IValidator<CreateRoleRequest>, CreateRoleRequestValidator>();
        services.AddScoped<IValidator<UpdateRoleRequest>, UpdateRoleRequestValidator>();
        services.AddScoped<IValidator<CreateWorkspaceRequest>, CreateWorkspaceRequestValidator>();
        services.AddScoped<IValidator<UpdateWorkspaceRequest>, UpdateWorkspaceRequestValidator>();
        services.AddScoped<IValidator<DeleteUserRequest>, DeleteUserRequestValidator>();
        services.AddScoped<IValidator<InviteUserRequest>, InviteUserRequestValidator>();
        services.AddScoped<IValidator<UpdateUserRequest>, UpdateUserRequestValidator>();
    }

    /// <summary>
    /// Внедрение зависимостей сервисов
    /// </summary>
    /// <param name="services"></param>
    private static void InitServices(this IServiceCollection services)
    {
        services.AddScoped<IWorkspaceService, WorkspacesService>();
        services.AddScoped<IWorkspacePositionsService, WorkspacePositionsService>();
        services.AddScoped<IWorkspaceRolesService, WorkspaceRolesService>();
        services.AddScoped<IWorkspaceDirectoryService, WorkspaceDirectoryService>();
        services.AddScoped<IClaimsService, ClaimsService>();
    }
}