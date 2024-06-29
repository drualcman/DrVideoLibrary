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
        services.AddScoped<IndexViewModel>();
        services.AddScoped<WatchlistViewModel>();

        services.AddLocalization();
        return services;
    }

    public async static Task SetDefaultCulture(this WebAssemblyHost host)
    {
        const string defaultCulture = "es-ES";

        IJSRuntime js = host.Services.GetRequiredService<IJSRuntime>();
        string result = await js.InvokeAsync<string>("blazorCulture.get");
        CultureInfo culture = CultureInfo.GetCultureInfo(defaultCulture);

        if (result == null)
            await js.InvokeVoidAsync("blazorCulture.set", defaultCulture);
        else
            culture = new CultureInfo(result);

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }
}
