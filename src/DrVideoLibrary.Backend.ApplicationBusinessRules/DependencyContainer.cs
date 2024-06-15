namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
    public static IServiceCollection AddApplicationOptions(this IServiceCollection services,
        Action<ConnectionStringsOptions> connectionStringsOptions,
        Action<StorageOptions> storageOptions)
    {
        services.Configure(connectionStringsOptions);
        services.Configure(storageOptions);
        return services;
    }
}
