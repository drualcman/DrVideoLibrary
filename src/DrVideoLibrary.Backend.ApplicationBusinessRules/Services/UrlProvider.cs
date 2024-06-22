namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Services;
internal class UrlProvider : IUrlProvider
{
    private readonly string _baseUrl;

    public UrlProvider(IOptions<NotificationOptions> options)
    {
        _baseUrl = options.Value.ClientUri;
    }

    public string GetUrl(string path)
    {
        return $"{_baseUrl}{path}";
    }
}
