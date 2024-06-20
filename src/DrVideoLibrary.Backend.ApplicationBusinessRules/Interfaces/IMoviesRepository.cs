namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces;
public interface IMoviesRepository
{
    Task AddMovie(Movie data);
    Task<IEnumerable<Movie>> GetAll();
    Task<IEnumerable<Movie>> GetAllByActors(string[] actors);
    Task<IEnumerable<Movie>> GetAllByDirectors(string[] directors);
    Task<IEnumerable<Movie>> GetAllByCategories(string[] categories);
    Task<Movie> GetMovieById(string id);
    Task RegisterWatchingNow(WatchingNowDto data);
    Task<WatchingNowDto> GetWatchingNow();
    Task<int> GetTotalViews(string movieId);

}
