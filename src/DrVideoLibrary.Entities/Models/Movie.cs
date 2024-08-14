namespace DrVideoLibrary.Entities.Models;
public class Movie : IMovie
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Cover { get; set; }
    public string OriginalTitle { get; set; }
    public int Year { get; set; }
    public string Description { get; set; }
    public byte Rate { get; set; }
    public int Duration { get; set; }
    public int TotalViews { get; set; }
    public List<string> Categories { get; set; }
    public List<string> Directors { get; set; }
    public List<string> Actors { get; set; }
}
