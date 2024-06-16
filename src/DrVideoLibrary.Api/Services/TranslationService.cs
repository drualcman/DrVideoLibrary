namespace DrVideoLibrary.Api.Services;

internal class TranslationService : ITranslateService
{
    private readonly HttpClient Client;
    private readonly TranslationOptions Options;

    public TranslationService(HttpClient httpClient, IOptions<TranslationOptions> options)
    {
        Client = httpClient;
        Client.BaseAddress = new Uri(options.Value.TranslationUrl);
        Options = options.Value;
    }

    public async Task<string> TranslateTextAsync(string text, string fromLanguage, string targetLanguage)
    {
        Dictionary<string, string> requestBody = new Dictionary<string, string>
        {
            { "auth_key", Options.TranslationApiKey },
            { "text", text },
            { "source_lang", fromLanguage },
            { "target_lang", targetLanguage }
        };

        FormUrlEncodedContent requestContent = new FormUrlEncodedContent(requestBody);
        HttpResponseMessage response = await Client.PostAsync("translate", requestContent);
        response.EnsureSuccessStatusCode();

        string responseContent = await response.Content.ReadAsStringAsync();
        TranslationResult translationResult = JsonSerializer.Deserialize<TranslationResult>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        string result = text;
        if (translationResult != null)
            result = translationResult.Translations.FirstOrDefault()?.Text;

        return result;
    }

    private class TranslationResult
    {
        public List<Translation> Translations { get; set; }
    }

    private class Translation
    {
        public string Text { get; set; }
    }
}

