namespace Microsoft.Extensions.DependencyInjection;
public static partial class DependencyContainer
{
    public static IServiceCollection AddCacheServices(this IServiceCollection services,
        Action<CacheDbOptions> cacheDbOptions = null)
    {
        services.Configure(cacheDbOptions);
        services.AddBlazorIndexedDbContext<MoviesContext>(true);
        services.AddSingleton<MoviesCacheService>();
        return services;
    }
}
