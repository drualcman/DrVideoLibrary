namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Options;
public class SearchMovieEnglishOption
{
    public string SearchEnglishUrl { get; set; }
    public string SearchEnglishApiKey { get; set; }
    public SearchMovieLang Language => SearchMovieLang.EN;
}
