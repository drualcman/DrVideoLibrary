namespace DrVideoLibrary.Razor.ViewModels;
public abstract class PaginatorViewModel<TModel>(IOptions<PaginatorOptions> options)
{
    public PaginationObjectHandler<TModel> Paginator { get; private set; }
    public IEnumerable<TModel> Items => Paginator?.Items;
    public bool HasItems => Paginator?.HasItems ?? false;

    protected virtual void InitializePaginator(IReadOnlyList<TModel> data)
    {
        Paginator = new PaginationObjectHandler<TModel>(data, options.Value.ElementsPerPage);
    }

}
