namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
    public static IServiceCollection AddBackendServices(this IServiceCollection services)
    {
        services.AddHttpClient<ISearchMovieService<SearchMovieInSpanish>, SearchMovieSpanishService>();
        services.AddHttpClient<ISearchMovieService<SearchMovieInEnglish>, SearchMovieEnglishService>();
        services.AddHttpClient<ITranslateService, TranslationService>();
        return services;
    }
}
