namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.GetMovieById;
public interface IGetMovieByIdOutputPort
{
    Movie Content { get; }
    Task Handle(Movie data);
}
