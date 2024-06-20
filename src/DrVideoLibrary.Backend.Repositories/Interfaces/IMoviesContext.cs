namespace DrVideoLibrary.Backend.Repositories.Interfaces;
public interface IMoviesContext
{
    Task AddMovie(MovieModel data);
    Task<IEnumerable<MovieModel>> GetAll();
    Task<IEnumerable<MovieModel>> GetAllByActors(string[] actors);
    Task<IEnumerable<MovieModel>> GetAllByDirectors(string[] directors);
    Task<IEnumerable<MovieModel>> GetAllByCategories(string[] categories);
    Task RegisterWatchingNow(RegisterView data);
    Task<RegisterView> GetWatchingNow();
    Task<IEnumerable<RegisterView>> GetTotalViews(string movieId);
    Task<MovieModel> GetMovieById(string id);
}
