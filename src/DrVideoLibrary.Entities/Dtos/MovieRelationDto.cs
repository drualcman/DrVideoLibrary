namespace DrVideoLibrary.Entities.Dtos;
public class MovieRelationDto : IMovie
{
    public string Id { get; set; }

    public string Cover { get; set; }

    public string Title { get; set; }
    public List<string> Directors { get; set; }
    public List<string> Actors { get; set; }

}
