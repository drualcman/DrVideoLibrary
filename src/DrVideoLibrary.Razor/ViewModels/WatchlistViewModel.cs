namespace DrVideoLibrary.Razor.ViewModels;
public class WatchlistViewModel
{
    public List<WatchedCard> Watcheds { get; private set; }
    public int TotalMinutes => Watcheds.Sum(m => m.Duration);
    public bool IsReady { get; private set; }

    public async ValueTask GetList()
    {
        Watcheds =
        [
            new WatchedCard(Guid.NewGuid().ToString(), "Start Wars Episodio I", "", 215, 80),
            new WatchedCard(Guid.NewGuid().ToString(), "Start Wars Episodio II", "", 215, 75),
            new WatchedCard(Guid.NewGuid().ToString(), "Start Wars Episodio III", "", 215, 65),
            new WatchedCard(Guid.NewGuid().ToString(), "Start Wars Episodio IV", "", 215, 100),
            new WatchedCard(Guid.NewGuid().ToString(), "Start Wars Episodio V", "", 215, 95),
            new WatchedCard(Guid.NewGuid().ToString(), "Start Wars Episodio VI", "", 215, 100),
            new WatchedCard(Guid.NewGuid().ToString(), "Start Wars Episodio VII", "", 215, 50),
            new WatchedCard(Guid.NewGuid().ToString(), "Start Wars Episodio VIII", "", 215, 90),
            new WatchedCard(Guid.NewGuid().ToString(), "Start Wars Episodio IX", "", 215, 85),
        ];
        IsReady = true;
        await ValueTask.CompletedTask;
    }
}
