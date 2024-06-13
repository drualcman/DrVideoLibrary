namespace DrVideoLibrary.Razor.Handlers;

public class CustomAuthorizationMessageHandler(IAccessTokenProvider tokenProvider, 
    IStringLocalizer<ResourceCustomAuthorizationMessageHandler> localizer) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        await AddAuthorizationHeaderIfAvailable(request);
        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
        ThrowIfUnauthorized(response);
        return response;
    }

    private async Task AddAuthorizationHeaderIfAvailable(HttpRequestMessage request)
    {
        AccessTokenResult tokenResult = await tokenProvider.RequestAccessToken();
        if (tokenResult.TryGetToken(out AccessToken token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
    }

    private void ThrowIfUnauthorized(HttpResponseMessage response)
    {
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            throw new UnauthorizedAccessException(localizer.GetString(nameof(ResourceCustomAuthorizationMessageHandler.UnauthorizedAccessExceptionMessage)));
    }
}

