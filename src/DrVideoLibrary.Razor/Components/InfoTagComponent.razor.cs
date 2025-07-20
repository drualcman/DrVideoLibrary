namespace DrVideoLibrary.Razor.Components;
public partial class InfoTagComponent
{
    [Inject] MoviesCacheService CacheService { get; set; }
    [Parameter] public RelativeType Type { get; set; }
    [Parameter] public string Query { get; set; }
    [Parameter] public bool IsActive { get; set; } = false;
    string Name = string.Empty;
    MovieCounter Movie;
    bool ShowInfo;
    bool ShowCovers;

    protected override void OnParametersSet()
    {
        if(string.IsNullOrWhiteSpace(Name))
        {
            Name = Query;
            MovieCounter[] relativeMovies = CacheService.GetRelatives(Type);
            Movie = relativeMovies.FirstOrDefault(m => m.Name.Equals(Name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}