namespace DrVideoLibrary.Razor.Cache.Contexts;
public class MoviesContext : StoreContext<MoviesContext>
{
    public MoviesContext(IJSRuntime js, IOptions<CacheDbOptions> options) : base(js, options.Value.ContextSettings)
    {
    }
    public StoreSet<MovieCardModel> Movies { get; set; }
}
