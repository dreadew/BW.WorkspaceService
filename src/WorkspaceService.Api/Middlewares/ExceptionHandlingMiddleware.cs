using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WorkspaceService.Domain.Constants;
using WorkspaceService.Domain.Exceptions;

namespace WorkspaceService.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IStringLocalizer _localizer;

    public ExceptionHandlingMiddleware(RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IStringLocalizer<ExceptionHandlingMiddleware> localizer)
    {
        _next = next;
        _logger = logger;
        _localizer = localizer;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AlreadyExistsException ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.Conflict, _localizer[ex.Message]);
        }
        catch (NotFoundException ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.NotFound, _localizer[ex.Message]);
        }
        catch (ServiceException ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, _localizer[ex.Message], ex.visibleToUser);
        }
        catch (ForbiddenException ex)
        {
            var message = _localizer[ex.ResourceKey, ex.Target, ex.ExpectedClaim];
            await HandleExceptionAsync(context, HttpStatusCode.Forbidden, message);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, _localizer[ExceptionResourceKeys.UnexpectedError]);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode,
        string message, bool? isVisible = null)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var problemDetails = new ProblemDetails()
        {
            Status = (int)statusCode,
            Title = statusCode.ToString(),
            Instance = context.Request.Path
        };

        if (isVisible.HasValue && isVisible.Value)
        {
            problemDetails.Detail = message;
        }
        else
        {
            problemDetails.Detail = _localizer[ExceptionResourceKeys.UnexpectedError];
        }

        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}