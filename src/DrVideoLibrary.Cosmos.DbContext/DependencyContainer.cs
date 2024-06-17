namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
    public static IServiceCollection AddContextServices(this IServiceCollection services)
    {
        services.AddScoped<IMoviesContext, MoviesContext>();
        return services;
    }
}
