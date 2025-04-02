using System.Net;
using Microsoft.AspNetCore.Mvc;
using WorkspaceService.Domain.Exceptions;

namespace WorkspaceService.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AlreadyExistsException ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.Conflict, ex.Message);
        }
        catch (NotFoundException ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (ServiceException ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, ex.Message,
                ex.visibleToUser);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, ex.Message);
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
            problemDetails.Detail = "An unexpected error occured. Please try again later.";
        }

        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}