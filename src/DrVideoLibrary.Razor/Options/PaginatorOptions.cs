namespace DrVideoLibrary.Razor.Options;
public class PaginatorOptions
{
    public const string SectionKey = nameof(PaginatorOptions);

    public int ElementsPerPage { get; set; } = 5;
}
