namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.GetMovieById;
public interface IGetMovieByIdController
{
    Task<Movie> GetMovieById(string id);
}
