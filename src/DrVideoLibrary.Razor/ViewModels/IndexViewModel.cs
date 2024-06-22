namespace DrVideoLibrary.Razor.ViewModels;
public class IndexViewModel : PaginatorViewModel<ListCard>
{
    readonly MoviesCacheService CacheService;
    readonly ApiClient Client;
    public IndexViewModel(MoviesCacheService cacheService, ApiClient client, IOptions<PaginatorOptions> options) : base(options)
    {
        CacheService = cacheService;
        Client = client;
    }
    public int TotalMovies { get; private set; }
    public bool IsReady { get; private set; }

    public async ValueTask GetList()
    {
        List<ListCard> movies = await CacheService.GetList();
        TotalMovies = movies.Count;
        InitializePaginator(movies);
        IsReady = true;
        await ValueTask.CompletedTask;
    }

    public async Task StartPlayMovie(string movieId)
    {
        await Client.RegisterWatchingNowAsync(movieId);
    }
}
