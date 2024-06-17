namespace DrVideoLibrary.Entities.Models;
public class SearchMovieResult : IMovie
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string OriginalTitle { get; set; }
    public string Year { get; set; }
    public string Cover { get; set; }
    public string ReleaseDate { get; set; }
}
