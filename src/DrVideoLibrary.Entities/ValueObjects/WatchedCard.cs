﻿namespace DrVideoLibrary.Entities.ValueObjects;
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
    }
}
