namespace DrVideoLibrary.Razor.ViewModels;
public class WatchlistViewModel : PaginatorViewModel<WatchedCard>
{
    public WatchlistViewModel(IOptions<PaginatorOptions> options) : base(options)
    {
    }

    public TimeSpanResult TotalTime { get; private set; } 
    public bool IsReady { get; private set; }

    public async ValueTask GetList()
    {
        List<WatchedCard> watcheds =
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
        for (int i = 0; i < 100; i++)
        {
            watcheds.Add(new WatchedCard(Guid.NewGuid().ToString(), "Popelle el marino", "", 215, 80));
        }
        int totalMinutes = watcheds.Sum(m => m.Duration);
        TotalTime = TimeSpanResult.FromMinutes(totalMinutes);
        InitializePaginator(watcheds);
        IsReady = true;
        await ValueTask.CompletedTask;
    }
}
