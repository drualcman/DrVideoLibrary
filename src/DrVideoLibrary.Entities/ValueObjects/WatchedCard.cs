namespace DrVideoLibrary.Entities.ValueObjects;
public class WatchedCard : IMovie
{
    public string Id { get; }
    public string Title { get; }
    public string Cover { get; set; }
    public int Duration { get; }
    public byte Rate { get; }
    public int Times { get; }

    public WatchedCard(string id, string title, string cover, int duration, byte rate, int times)
    {
        Id = id;
        Title = title;
        Cover = cover;
        Duration = duration;
        Rate = rate;
        Times = times;
    }
}
