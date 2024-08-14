namespace DrVideoLibrary.Razor.Cache.Contexts;
internal class MoviesContext : StoreContext<MoviesContext>
{
    public MoviesContext(IJSRuntime js, IOptions<CacheDbOptions> options) : base(js, options.Value.ContextSettings)
    {
    }

    public StoreSet<MovieCardModel> Movies { get; set; }
    public StoreSet<ActorRelationModel> Actors { get; set; }
    public StoreSet<DirectorRelationModel> Directors { get; set; }
}
