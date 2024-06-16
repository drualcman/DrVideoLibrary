namespace DrVideoLibrary.Razor.Pages;
public partial class AddMovie
{
    private Movie Movie;

    private void OnMovieSelect(Movie selection)
    {
        Movie = selection;
    }

    private void SaveMovie()
    {
        Movie = null;
    }

}
