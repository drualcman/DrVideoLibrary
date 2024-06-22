namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Options;
public class SearchMovieSpanishOption
{
    public const string SectionKey = nameof(SearchMovieSpanishOption);
    public string SearchSpanishUrl { get; set; }
    public string SearchSpanishApiKey { get; set; }
    public SearchMovieLang Language => SearchMovieLang.ES;
}
