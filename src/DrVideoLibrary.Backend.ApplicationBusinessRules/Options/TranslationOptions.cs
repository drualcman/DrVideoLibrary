namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Options;
public class TranslationOptions
{
    public const string SectionKey = nameof(TranslationOptions);
    public string TranslationUrl { get; set; }
    public string TranslationApiKey { get; set; }
}
