using DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
    public static IServiceCollection AddBlobStorageServices(this IServiceCollection services)
    {
        services.AddScoped<IFileContent, StorageImageService>();
        return services;
    }
}
