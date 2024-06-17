namespace DrVideoLibrary.Backend.Repositories.Entities;
public class RegisterView
{
    public string Id { get; set; }
    public string MovieId { get; set; }
    public DateTime Start { get; set; }

    public RegisterView(string movieId, DateTime start)
    {
        Id = Guid.NewGuid().ToString();
        MovieId = movieId;
        Start = start;
    }
}
