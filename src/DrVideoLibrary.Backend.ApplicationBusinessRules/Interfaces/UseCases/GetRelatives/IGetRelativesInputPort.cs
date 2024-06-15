namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.GetRelatives;
public interface IGetRelativesInputPort
{
    Task Handle(string id);
}
