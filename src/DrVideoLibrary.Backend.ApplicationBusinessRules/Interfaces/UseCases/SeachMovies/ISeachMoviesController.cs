namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.SeachMovies;
public interface ISeachMoviesController
{
    Task<IEnumerable<SearchMovieResult>> SearchMovies(string text, string lang);
}
