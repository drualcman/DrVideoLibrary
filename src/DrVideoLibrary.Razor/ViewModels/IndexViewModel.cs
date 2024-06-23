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
    public IEnumerable<Category> Categories { get; private set; }
    public string SelectedCategory {  get; private set; } 

    public async Task GetList()
    {
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

    public bool IsSelectedCategory(string category) =>
        category.Equals(SelectedCategory, StringComparison.InvariantCultureIgnoreCase);

    public async Task FilterbyCategory(string category)
    {
        SelectedCategory = category;
        if (!string.IsNullOrEmpty(category))
        {
            IEnumerable<ListCard> movies = await CacheService.GetList();
            movies = movies.Where(x => x.Categories.Any(c => c.Equals(category, StringComparison.InvariantCultureIgnoreCase)));
            TotalMovies = movies.Count();
            InitializePaginator(movies.ToList());
        }
        else await GetList();
    }
}
