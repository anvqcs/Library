using Library.Infrastructure.Middleware;

namespace Library.WebAPI.Configurations
{
    public static class MiddlewareConfig
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
