namespace DrVideoLibrary.Entities.Interfaces;
public interface ISearchMovieService<TLang>
{
    Task<IEnumerable<SearchMovieResult>> SearchMovies(string text);
    Task<SearchPersonResult> SearchActor(string text);
    Task<Movie> GetMovieDetails(string id);
}
