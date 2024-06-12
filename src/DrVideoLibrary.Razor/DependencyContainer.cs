using DrVideoLibrary.Razor.Providers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyContainer
{
    public static IServiceCollection AddServices(this IServiceCollection services,
        Action<RemoteAuthenticationOptions<OidcProviderOptions>> authOptions = null)
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
        services.AddScoped<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
        services.AddScoped<WatchlistViewModel>();
        return services;
    }
}
