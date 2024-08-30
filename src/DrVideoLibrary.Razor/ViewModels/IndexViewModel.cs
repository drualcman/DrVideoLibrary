namespace DrVideoLibrary.Razor.ViewModels;
internal class IndexViewModel : PaginatorViewModel<ListCard>
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
    public bool IsFiltering { get; private set; }
    public IEnumerable<Category> Categories { get; private set; }
    public string SelectedCategory { get; private set; }

    public async Task GetList()
    {
        IsFiltering = true;
        IEnumerable<ListCard> movies = await CacheService.GetList();
        TotalMovies = movies.Count();
        InitializePaginator(movies.ToList());
        Categories = movies
            .SelectMany(x => x.Categories)
            .GroupBy(category => category)
            .Select(grup => new Category
            {
                Name = grup.Key,
                Count = grup.Count()
            });
        IsReady = true;
        IsFiltering = false;
    }

    public async Task StartPlayMovie(string movieId, string lang)
    {
        await Client.RegisterWatchingNowAsync(movieId, lang);
    }

    public async Task SeachMovie(string movie)
    {
        SelectedCategory = string.Empty;
        if (!string.IsNullOrEmpty(movie))
        {
            await ExecuteFilter(x => x.Title.Contains(movie, StringComparison.InvariantCultureIgnoreCase));
        }
        else await GetList();
    }

    public bool IsSelectedCategory(string category) =>
        category.Equals(SelectedCategory, StringComparison.InvariantCultureIgnoreCase);

    public async Task FilterbyCategory(string category)
    {
        SelectedCategory = category;
        if (!string.IsNullOrEmpty(category))
        {
            await ExecuteFilter(x => x.Categories.Any(c => c.Equals(category, StringComparison.InvariantCultureIgnoreCase)));
        }
        else await GetList();
    }

    private async Task ExecuteFilter(Func<ListCard, bool> query)
    {
        IsFiltering = true;
        IEnumerable<ListCard> movies = await CacheService.GetList();
        movies = movies.Where(query);
        TotalMovies = movies.Count();
        InitializePaginator(movies.ToList());
        IsFiltering = false;
    }
}
