namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.SeachMovies;
public interface ISeachMoviesInputPort
{
    Task<IEnumerable<SearchMovieResult>> SearchMovies(string text, string lang);
}
