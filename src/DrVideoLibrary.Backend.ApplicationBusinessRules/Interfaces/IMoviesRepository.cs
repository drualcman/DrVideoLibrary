namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces;
public interface IMoviesRepository
{
    Task AddMovie(Movie data);
    Task<IEnumerable<Movie>> GetAll();
    Task<Movie> GetById(string id);
    Task RegisterWatchingNow(string id);

}
