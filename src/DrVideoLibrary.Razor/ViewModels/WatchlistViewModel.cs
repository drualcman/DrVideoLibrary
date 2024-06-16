namespace DrVideoLibrary.Razor.ViewModels;
public class WatchlistViewModel : PaginatorViewModel<WatchedCard>
{
    readonly ApiClient Client;
    public WatchlistViewModel(ApiClient client, IOptions<PaginatorOptions> options) : base(options)
    {
        Client = client;
    }

    public TimeSpanResult TotalTime { get; private set; } 
    public bool IsReady { get; private set; }

    public async ValueTask GetList()
    {
        List<WatchedCard> watcheds = new List<WatchedCard>(await Client.GetWatchedListAsync());
        int totalMinutes = watcheds.Sum(m => m.Duration);
        TotalTime = TimeSpanResult.FromMinutes(totalMinutes);
        InitializePaginator(watcheds);
        IsReady = true;
        await ValueTask.CompletedTask;
    }
}
