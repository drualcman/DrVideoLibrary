
namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.SeachMovies;
internal class SeachMoviesController : ISeachMoviesController
{
    readonly ISeachMoviesInputPort Input;

    public SeachMoviesController(ISeachMoviesInputPort input)
    {
        Input = input;
    }

    public Task<IEnumerable<SearchMovieResult>> SearchMovies(string text, string lang)  =>
        Input.SearchMovies(text, lang);
}
