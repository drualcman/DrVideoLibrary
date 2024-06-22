namespace Microsoft.Extensions.DependencyInjection;
public static partial class DependencyContainer
{
    public static IServiceCollection AddServices(this IServiceCollection services,
        Action<RemoteAuthenticationOptions<OidcProviderOptions>> authOptions = null,
        Action<ApiClientOptions> apiOptions = null,
        Action<SearchMovieOptions> searchMovieOptions = null,
        Action<PaginatorOptions> paginatorOptions = null,
        Action<CacheDbOptions> cacheDbOptions = null,
        Action<PushNotificationOptions> pushNotificationOptions = null)
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
        services.Configure(searchMovieOptions);
        services.Configure(paginatorOptions);
        services.Configure(pushNotificationOptions);

        services.AddCacheServices(cacheDbOptions);

        services.AddHttpClient();
        services.AddHttpClient<ApiClient>();
        services.AddHttpClient<INotificationClient, NotificationsClient>();
        services.AddHttpClient<ISearchMovieService<SearchMovieSpanishService>, SearchMovieSpanishService>();
        services.AddHttpClient<ISearchMovieService<SearchMovieEnglishService>, SearchMovieEnglishService>();
        services.AddLocalization();
        services.AddScoped<IndexViewModel>();
        services.AddScoped<WatchlistViewModel>();
        CultureInfo culture = new CultureInfo("es");
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        return services;
    }
}
