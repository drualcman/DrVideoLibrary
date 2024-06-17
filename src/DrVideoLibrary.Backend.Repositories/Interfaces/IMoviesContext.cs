namespace DrVideoLibrary.Backend.Repositories.Interfaces;
public interface IMoviesContext
{
    Task AddMovie(MovieModel data);
}
