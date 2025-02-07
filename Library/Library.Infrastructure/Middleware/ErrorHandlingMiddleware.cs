using Library.Core.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Library.Infrastructure.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception has occurred.");

            switch (ex)
            {
                case NotFoundException:
                    httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                    await httpContext.Response.WriteAsync("Resource not found");
                    break;
                case ForbiddenAccessException:
                    httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await httpContext.Response.WriteAsync("You do not have permission to access this resource");
                    break;
                case UnauthorizedAccessException:
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await httpContext.Response.WriteAsync("Unable to authorize your account");
                    break;
                case InvalidOperationException:
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await httpContext.Response.WriteAsync("Bad request");
                    break;
                default:
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await httpContext.Response.WriteAsync("An unexpected error occurred");
                    break;
            }
        }
    }
}