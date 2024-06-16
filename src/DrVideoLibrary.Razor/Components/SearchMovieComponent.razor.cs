namespace DrVideoLibrary.Razor.Components;
public partial class SearchMovieComponent<TSearchService> where TSearchService : ISearchMovieService<TSearchService>
{
    [Inject] ISearchMovieService<TSearchService> SearchMovieService { get; set; }
    [Parameter] public EventCallback<Movie> OnSelect { get; set; }

    string SearchTitle;
    string Messages;
    IEnumerable<SearchMovieResult> SearchResults;
    async Task SearchMovie()
    {
        Messages = string.Empty;
        if (string.IsNullOrEmpty(SearchTitle))
        {
            Messages = Localizer.GetString(nameof(ResourceSearchMovieComponent.SearchTitleErrorEmptyMessage));
        }
        else
        {
            SearchResults = await SearchMovieService.SearchMovies(SearchTitle);
            if (SearchResults is null)
            {
                Messages = Localizer.GetString(nameof(ResourceSearchMovieComponent.SearchMoviesErrorMessage));
            }
        }
    }

    async Task OnMovieSelect(SearchMovieResult selection)
    {
        Messages = string.Empty;
        Movie movieDetails = await SearchMovieService.GetMovieDetails(selection.Id);
        if (movieDetails != null)
        {
            if (OnSelect.HasDelegate)
                await OnSelect.InvokeAsync(movieDetails);
        }
        else
        {
            Messages = Localizer.GetString(nameof(ResourceSearchMovieComponent.OnMovieSelectErrorMessage));
        }
        SearchResults = null;
    }

}