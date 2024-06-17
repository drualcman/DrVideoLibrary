
namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.SeachMovies;
internal class SeachMoviesController : ISeachMoviesController
{
    readonly ISeachMoviesInputPort Input;

    public SeachMoviesController(ISeachMoviesInputPort input)
    {
        Input = input;
    }

    public async Task<IEnumerable<SearchMovieResult>> SearchMovies(string text, string lang)
    {
        List<SearchMovieResult> result = new List<SearchMovieResult>();
        var response = await Input.SearchMovies(text, lang);
        if (response is not null)
            result.AddRange(response);
        return result;        
    }
}
