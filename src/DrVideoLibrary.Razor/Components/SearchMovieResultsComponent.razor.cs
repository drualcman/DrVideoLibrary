namespace DrVideoLibrary.Razor.Components;
public partial class SearchMovieResultsComponent
{
    [Parameter] public IEnumerable<SearchMovieResult> SearchResults { get; set; }
    [Parameter] public EventCallback<SearchMovieResult> OnSelect { get; set; }

    private async Task OnMovieSelect(SearchMovieResult selection)
    {
        if (OnSelect.HasDelegate)
            await OnSelect.InvokeAsync(selection);
    }
}