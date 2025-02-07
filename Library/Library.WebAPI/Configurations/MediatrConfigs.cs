using System.Reflection;

namespace Library.WebAPI.Configurations;

public static class MediatrConfigs
{
    public static IServiceCollection AddMediatrConfigs(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        return services;
    }
}