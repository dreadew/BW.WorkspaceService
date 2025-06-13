using System.Globalization;
using AutoMapper;
using Common.Base.DTO.Entity;
using Common.Base.Services;
using Common.Base.Utils;
using Common.Services.Extensions;
using Common.Services.MappingActions;
using Common.Services.Mappings;
using Common.Services.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using WorkspaceService.Application.Mapping;
using WorkspaceService.Application.Services;
using WorkspaceService.Application.Validators.WorkspacePositions;
using WorkspaceService.Application.Validators.WorkspaceRoles;
using WorkspaceService.Application.Validators.Workspaces;
using WorkspaceService.Application.Validators.WorkspaceUsers;
using WorkspaceService.Domain.DTOs.WorkspacePositions;
using WorkspaceService.Domain.DTOs.WorkspaceRoles;
using WorkspaceService.Domain.DTOs.Workspaces;
using WorkspaceService.Domain.DTOs.WorkspaceUsers;
using WorkspaceService.Domain.Entities;
using WorkspaceService.Domain.Services;

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
        services.AddBaseDirectoryServices();
    }

    /// <summary>
    /// Внедрение зависимостей маппинга
    /// </summary>
    /// <param name="services"></param>
    private static void InitMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(WorkspaceProfile));
        services.AddAutoMapper(typeof(WorkspacePositionProfile));
        services.AddAutoMapper(typeof(WorkspaceRoleProfile));
        services.AddAutoMapper(typeof(WorkspaceRoleClaimProfile));
        services.AddAutoMapper(typeof(FileProfile));
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
        services.AddScoped<IWorkspaceService, Services.WorkspaceService>();
        services.AddScoped<IWorkspacePositionsService, WorkspacePositionService>();
        services.AddScoped<IWorkspaceRolesService, WorkspaceRoleService>();
        services.AddScoped<IWorkspaceRoleClaimsService, WorkspaceRoleClaimService>();
        services.AddScoped<IClaimsService, ClaimService>();
    }
}