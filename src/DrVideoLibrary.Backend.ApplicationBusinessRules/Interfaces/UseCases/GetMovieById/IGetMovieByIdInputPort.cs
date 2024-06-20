namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.GetMovieById;
public interface IGetMovieByIdInputPort
{
    Task Handle(string id); 
}
