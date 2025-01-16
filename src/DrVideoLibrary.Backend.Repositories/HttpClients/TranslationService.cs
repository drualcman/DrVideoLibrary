namespace DrVideoLibrary.Backend.Repositories.HttpClients;

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
        string result = text;
        string from = GetTwoLetterLanguageCode(fromLanguage);
        string to = GetTwoLetterLanguageCode(targetLanguage);
        if (CanTransalte(from, to))
        {
            Dictionary<string, string> requestBody = new Dictionary<string, string>
            {
                { "auth_key", Options.TranslationApiKey },
                { "text", text },
                { "source_lang", from },
                { "target_lang", to }
            };

            FormUrlEncodedContent requestContent = new FormUrlEncodedContent(requestBody);
            HttpResponseMessage response = await Client.PostAsync("translate", requestContent);
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                TranslationResult translationResult = JsonSerializer.Deserialize<TranslationResult>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (translationResult != null)
                    result = translationResult.Translations.FirstOrDefault()?.Text;
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();
                Console.WriteLine(error);
            }
        }
        return result;
    }

    private string GetTwoLetterLanguageCode(string language)
    {
        string result = language;
        if (!string.IsNullOrWhiteSpace(language))
        {
            result = language.Length >= 2 ? language.Substring(0, 2).ToLowerInvariant() : language.ToLowerInvariant();
        }
        return result;
    }

    private bool CanTransalte(string from, string target)
    {
        bool result;
        if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(target)) result = false;
        else if (from.Equals(target, StringComparison.OrdinalIgnoreCase)) result = false;
        else result = true;
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

