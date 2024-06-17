namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.AddMovie;
public interface IAddMovieInputPort
{
    Task AddMovie(Movie data);
}
