namespace DrVideoLibrary.Razor.Pages;
public partial class AddMovie
{
    [Inject] ApiClient Client { get; set; }

    private Movie Movie;

    private void OnMovieSelect(Movie selection)
    {
        Movie = selection;
    }

    private async Task SaveMovie()
    {
        await Client.AddMovieAsync(Movie);
        Movie = null;
    }

}
