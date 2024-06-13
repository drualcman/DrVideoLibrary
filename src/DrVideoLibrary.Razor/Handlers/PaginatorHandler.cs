namespace DrVideoLibrary.Razor.Handlers;

public class PaginatorHandler
{
    readonly int TotalElements;
    protected readonly int PageSize;

    public PaginatorHandler(int totalElements, int pageSize)
    {
        TotalElements = totalElements;
        PageSize = pageSize;
    }

    public int ActualPage { get; private set; } = 1;
    public virtual int TotalPages => (int)Math.Ceiling((double)TotalElements / PageSize);

    public virtual void ToPage(int page)
    {
        if (page < 1) ActualPage = 1;
        else if (page > TotalPages) ActualPage = TotalPages;
        else ActualPage = page;
    }

    public bool HasPreviousPage => ActualPage > 1;
    public bool HasNextPage => ActualPage < TotalPages;

}
