namespace DrVideoLibrary.Razor.Pages;
public partial class AddMovie
{
    [Inject] ApiClient Client { get; set; }

    private Movie Movie;

    private void OnMovieSelect(Movie selection)
    {
        Movie = selection;
    }

    void NewMovie()
    {
        Movie = new Movie
        {
            Categories = [],
            Directors = [],
            Actors = [],
            Year = DateTime.Today.Year
        };
    }

    private async Task SaveMovie()
    {
        await Task.Delay(1);
        //await Client.AddMovieAsync(Movie);
        Movie = null;
    }

}
