namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
    public static IServiceCollection AddContextServices(this IServiceCollection services)
    {
        services.AddScoped<IMoviesContext, MoviesContext>();
        services.AddScoped<INotificationContext, NotificationContext>();
        return services;
    }
}
