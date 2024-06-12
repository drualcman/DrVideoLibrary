using DrVideoLibrary.Razor.Providers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyContainer
{
    public static IServiceCollection AddServices(this IServiceCollection services, Action<RemoteAuthenticationOptions<OidcProviderOptions>> authOptions)
    {
        services.AddAuthorizationCore();                          //can remove is use OAuth
        services.AddCascadingAuthenticationState();               //can remove is use OAuth
        //services.AddOidcAuthentication(authOptions);              //to use with OAuth
        services.AddScoped<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
        return services;
    }
}
