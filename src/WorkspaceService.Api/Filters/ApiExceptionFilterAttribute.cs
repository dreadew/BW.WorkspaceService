using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WorkspaceService.Domain.Exceptions;

namespace WorkspaceService.Api.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;
    private readonly ILogger<ApiExceptionFilterAttribute> _logger;
    private readonly IHostEnvironment _environment;

    public ApiExceptionFilterAttribute(
        ILogger<ApiExceptionFilterAttribute> logger, 
        IHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
        
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            { typeof(AlreadyExistsException), HandleAlreadyExistsException },
            { typeof(NotFoundException), HandleNotFoundException },
            { typeof(ServiceException), HandleServiceException }
        };
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);
        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        var type = context.Exception.GetType();
        
        _logger.LogError(context.Exception, "An error occurred: {Message}", context.Exception.Message);
        if (_exceptionHandlers.TryGetValue(type, out var handler))
        {
            handler.Invoke(context);
            return;
        }
        
        HandleUnknownException(context);
    }

    private void HandleAlreadyExistsException(ExceptionContext context)
    {
        var exception = (AlreadyExistsException)context.Exception;
        
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = "Resource Already Exists",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8",
            Detail = exception.Message,
            Instance = context.HttpContext.Request.Path
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status409Conflict
        };

        context.ExceptionHandled = true;
    }

    private void HandleNotFoundException(ExceptionContext context)
    {
        var exception = (NotFoundException)context.Exception;

        var details = new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Resource Not Found",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Detail = exception.Message,
            Instance = context.HttpContext.Request.Path
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status404NotFound
        };

        context.ExceptionHandled = true;
    }

    private void HandleServiceException(ExceptionContext context)
    {
        var exception = (ServiceException)context.Exception;

        var details = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal Server Error",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Instance = context.HttpContext.Request.Path
        };

        // Only show details if allowed
        if (exception.visibleToUser)
        {
            details.Detail = exception.Message;
        } 
        else 
        {
            details.Detail = "An unexpected error occurred. Please try again later.";
        }

        // Add traceId for better debugging
        details.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true;
    }

    private void HandleUnknownException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An error occurred while processing your request.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Instance = context.HttpContext.Request.Path
        };

        // Only show exception details in development
        if (_environment.IsDevelopment())
        {
            details.Detail = context.Exception.ToString();
        }
        else
        {
            details.Detail = "An unexpected error occurred. Please try again later.";
        }

        // Add traceId for better debugging
        details.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true;
    }
}