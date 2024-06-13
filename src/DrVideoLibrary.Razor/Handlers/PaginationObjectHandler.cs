namespace DrVideoLibrary.Razor.Handlers;
public class PaginationObjectHandler<TData> : PaginatorHandler
{
    readonly IReadOnlyList<TData> Data;

    public PaginationObjectHandler(IReadOnlyList<TData> data, int pageSize) : base(data.Count, pageSize) =>
        Data = data;

    public bool HasItems => Data.Count > 0;
    public IEnumerable<TData> Items => Data.Skip(ToSkip).Take(PageSize);

    int ToSkip => (ActualPage - 1) * PageSize;
}
