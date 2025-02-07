using Library.Infrastructure;
using Library.UseCases;

namespace Library.WebAPI.Configurations;

public static class ServiceConfigs
{
    public static IServiceCollection AddServiceConfigs(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddInfrastructureServices(builder.Configuration)
            .AddAuthenticateConfiguration(builder.Configuration)
            .AddApplicationServices();
        return services;
    }
}