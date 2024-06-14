namespace DrVideoLibrary.Entities.Models;
public class Movie
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Cover { get; set; }
    public DateTime Released { get; set; }
    public string Prologo { get; set; }
    public byte Rate { get; set; }
    public int Duration { get; set; }
    public List<string> Categories { get; set; }
    public List<string> Directors { get; set; }
    public List<string> Actors { get; set; }
    public List<RelativeMovie> Relatives { get; set; }
}
