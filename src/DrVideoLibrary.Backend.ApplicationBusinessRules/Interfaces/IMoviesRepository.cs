namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces;
public interface IMoviesRepository
{
    Task AddMovie(Movie data);
}
