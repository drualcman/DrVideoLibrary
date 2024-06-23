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

    List<WatchedCard> Watcheds = null;

    public async ValueTask GetWatchList()
    {      
        if(Watcheds == null) 
            Watcheds = new List<WatchedCard>(await Client.GetWatchedListAsync());
    } 
    public async Task GetList()
    {
        await GetWatchList();
        int totalMinutes = Watcheds.Sum(m => m.Duration);
        TotalTime = TimeSpanResult.FromMinutes(totalMinutes);
        InitializePaginator(Watcheds);
        IsReady = true;
    }

    public async Task SeachMovie(string movie)
    {
        if (!string.IsNullOrEmpty(movie))
        {
            await GetWatchList();
            IEnumerable<WatchedCard> movies = Watcheds.Where(x => x.Title.Contains(movie, StringComparison.InvariantCultureIgnoreCase));
            int totalMinutes = movies.Sum(m => m.Duration);
            TotalTime = TimeSpanResult.FromMinutes(totalMinutes);
            InitializePaginator(movies.ToList());
        }
        else await GetList();
    }
}
