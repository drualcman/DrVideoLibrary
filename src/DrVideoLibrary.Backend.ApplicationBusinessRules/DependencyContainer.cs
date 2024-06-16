namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
    public static IServiceCollection AddApplicationOptions(this IServiceCollection services,
        Action<ConnectionStringsOptions> connectionStringsOptions,
        Action<SearchMovieSpanishOption> searchMovieSpanishOption,
        Action<SearchMovieEnglishOption> searchMovieEnglishOption,
        Action<TranslationOptions> translationOptions,
        Action<StorageOptions> storageOptions)
    {
        services.Configure(connectionStringsOptions);
        services.Configure(searchMovieSpanishOption);
        services.Configure(searchMovieEnglishOption);
        services.Configure(translationOptions);
        services.Configure(storageOptions);
        return services;
    }
}
