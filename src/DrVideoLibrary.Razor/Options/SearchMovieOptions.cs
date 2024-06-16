namespace DrVideoLibrary.Razor.Options;
public class SearchMovieOptions
{
    public const string SectionKey = nameof(SearchMovieOptions);
    public List<SearchMovieServiceOption> Services { get; set; }
}
