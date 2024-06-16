namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Options;
public class SearchMovieSpanishOption
{
    public string SearchSpanishUrl { get; set; }
    public string SearchSpanishApiKey { get; set; }
    public SearchMovieLang Language => SearchMovieLang.ES;
}
