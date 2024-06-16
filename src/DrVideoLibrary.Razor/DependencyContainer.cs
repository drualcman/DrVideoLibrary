﻿namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyContainer
{
    public static IServiceCollection AddServices(this IServiceCollection services,
        Action<RemoteAuthenticationOptions<OidcProviderOptions>> authOptions = null,
        Action<ApiClientOptions> apiOptions = null,
        Action<TranslationOptions> translationOptions = null,
        Action<SearchMovieOptions> searchMovieOptions = null,
        Action<PaginatorOptions> paginatorOptions = null)
    {
        if (authOptions == null)
        {
            services.AddAuthorizationCore();
            services.AddCascadingAuthenticationState();
            services.AddScoped<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
        }
        else
        {
            //to use with OAuth
            services.AddOidcAuthentication(authOptions);
        }
        services.Configure(apiOptions);
        services.Configure(translationOptions);
        services.Configure(searchMovieOptions);
        services.Configure(paginatorOptions);

        services.AddHttpClient();
        services.AddHttpClient<ApiClient>();
        services.AddHttpClient<ISearchMovieService<SearchMovieSpanishService>, SearchMovieSpanishService>();
        services.AddHttpClient<ISearchMovieService<SearchMovieEnglishService>, SearchMovieEnglishService>();
        services.AddHttpClient<ITranslateService, TranslationService>();
        services.AddLocalization();
        services.AddScoped<IndexViewModel>();
        services.AddScoped<WatchlistViewModel>();
        CultureInfo culture = new CultureInfo("es");
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        return services;
    }
}
