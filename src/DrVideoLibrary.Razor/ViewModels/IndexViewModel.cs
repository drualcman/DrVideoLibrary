namespace DrVideoLibrary.Razor.ViewModels;
public class IndexViewModel : PaginatorViewModel<ListCard>
{
    public IndexViewModel(IOptions<PaginatorOptions> options) : base(options)
    {
    }
    public int TotalMovies { get; private set; }
    public bool IsReady { get; private set; }

    public async ValueTask GetList()
    {
        List<ListCard> movies =
        [
            new ListCard(Guid.NewGuid().ToString(), "Un nacimiento de un ser muy especial en la tierra media de nuestro señor", "", 1976, ["miedo", "terror"]),
            new ListCard(Guid.NewGuid().ToString(), "El traviero", "", 1979, ["comedia", "drama"]),
            new ListCard(Guid.NewGuid().ToString(), "El colegio, que miedo", "", 1982, ["ciencia ficcion", "comedia", "accion", "drama"])
        ];

        for (int i = 0; i < 1000; i++)
        {
            movies.Add(new ListCard(Guid.NewGuid().ToString(), "El traviero", "", 1979, ["comedia", "drama"]));
        }
        TotalMovies = movies.Count;
        InitializePaginator(movies);
        IsReady = true;
        await ValueTask.CompletedTask;
    }
}
