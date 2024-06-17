namespace DrVideoLibrary.Backend.Repositories.Entities;
public class MovieModel
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string OriginalTitle { get; set; }
    public string Cover { get; set; }
    public int Year { get; set; }
    public string Description { get; set; }
    public byte Rate { get; set; }
    public int Duration { get; set; }
    public string[] Categories { get; set; }
    public string[] Directors { get; set; }
    public string[] Actors { get; set; }

    public Movie ToMovie() => new Movie
    {
        Id = Id,
        Title = Title,
        OriginalTitle = OriginalTitle,
        Cover = Cover,
        Year = Year,
        Description = Description,
        Rate = Rate,
        Duration = Duration,
        Categories = new List<string>(Categories),
        Directors = new List<string>(Directors),
        Actors = new List<string>(Actors),
        TotalViews = 0
    };

    public static MovieModel FromMovie(Movie movie) => new MovieModel
    {
        Id = movie.Id,
        Title = movie.Title,
        OriginalTitle = movie.OriginalTitle,
        Cover = movie.Cover,
        Year = movie.Year,
        Description = movie.Description,
        Rate = movie.Rate,
        Duration = movie.Duration,
        Categories = movie.Categories.ToArray(),
        Directors = movie.Directors.ToArray(),
        Actors = movie.Actors.ToArray()
    };
}
