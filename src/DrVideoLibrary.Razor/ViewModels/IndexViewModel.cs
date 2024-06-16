namespace DrVideoLibrary.Razor.ViewModels;
public class IndexViewModel : PaginatorViewModel<ListCard>
{
    readonly ApiClient Client;
    public IndexViewModel(ApiClient client, IOptions<PaginatorOptions> options) : base(options)
    {
        Client = client;
    }
    public int TotalMovies { get; private set; }
    public bool IsReady { get; private set; }

    public async ValueTask GetList()
    {
        List<ListCard> movies = new(await Client.GetMovies());
        TotalMovies = movies.Count;
        InitializePaginator(movies);
        IsReady = true;
        await ValueTask.CompletedTask;
    }
}
