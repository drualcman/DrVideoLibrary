namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces;
public interface ITranslateService
{
    Task<string> TranslateTextAsync(string text, string fromLanguage, string targetLanguage);
}
