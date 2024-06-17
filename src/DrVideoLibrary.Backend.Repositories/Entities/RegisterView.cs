namespace DrVideoLibrary.Backend.Repositories.Entities;
public class RegisterView
{
    public string Id { get; set; }
    public DateTime Start { get; set; }

    public RegisterView(string id, DateTime start)
    {
        Id = id;
        Start = start;
    }
}
