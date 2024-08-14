namespace DrVideoLibrary.Razor.Models;

public class MovieBasic : IMovie
{
    public string Id { get; set; }

    public string Cover { get; set; }

    public string Title { get; set; }

    public MovieBasic(string id, string cover, string title)
    {
        Id = id;
        Title = title;
        Cover = cover;
    }
}
