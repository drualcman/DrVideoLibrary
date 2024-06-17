namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces;
public interface IMoviesRepository
{
    Task AddMovie(Movie data);
    Task<IEnumerable<Movie>> GetAll();
    Task<Movie> GetMovieById(string id);
    Task RegisterWatchingNow(string id);
    Task<WatchingNowDto> GetWatchingNow();
    Task<int> GetTotalViews(string movieId);

}
