namespace DrVideoLibrary.Entities.ValueObjects;
public class WatchedCard
{
    public string Id { get; }
    public string Title { get; }
    public string Cover { get; }
    public int Duration { get; }
    public byte Rate { get; }

    public WatchedCard(string id, string title, string cover, int duration, byte rate)
    {
        Id = id;
        Title = title;
        Cover = cover;
        Duration = duration;
        Rate = rate;
        Cover = "https://books.community-mall.com/images/file070825676587736583778817187786848566707281006846872787106475657384.jpg";
    }
}
