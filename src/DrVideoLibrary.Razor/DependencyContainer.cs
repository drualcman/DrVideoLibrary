namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyContainer
{
    public static IServiceCollection AddServices(this IServiceCollection services,
        Action<RemoteAuthenticationOptions<OidcProviderOptions>> authOptions = null,
        Action<PaginatorOptions> paginatorOptions = null)
    {
        if (authOptions == null)
        {
            services.AddAuthorizationCore();
            services.AddCascadingAuthenticationState();
        }
        else
        {
            //to use with OAuth
            services.AddOidcAuthentication(authOptions);
        }
        services.Configure(paginatorOptions);
        services.AddLocalization();
        services.AddScoped<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
        services.AddScoped<IndexViewModel>();
        services.AddScoped<WatchlistViewModel>();
        CultureInfo culture = new CultureInfo("es");
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        return services;
    }
}
