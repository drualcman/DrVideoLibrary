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

    public async Task GetList()
    {
        IEnumerable<ListCard> movies = await CacheService.GetList();
        TotalMovies = movies.Count();
        InitializePaginator(movies.ToList());
        IsReady = true;
    }

    public async Task StartPlayMovie(string movieId)
    {
        await Client.RegisterWatchingNowAsync(movieId);
    }

    public async Task SeachMovie(string movie)
    {
        if (!string.IsNullOrEmpty(movie))
        {
            IEnumerable<ListCard> movies = await CacheService.GetList();
            movies = movies.Where(x => x.Title.Contains(movie, StringComparison.InvariantCultureIgnoreCase));
            TotalMovies = movies.Count();
            InitializePaginator(movies.ToList());
        }
        else await GetList();
    }
}
