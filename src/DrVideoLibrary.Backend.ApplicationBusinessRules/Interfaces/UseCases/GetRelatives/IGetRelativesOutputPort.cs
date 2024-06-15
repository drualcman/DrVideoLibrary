namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.GetRelatives;
public interface IGetRelativesOutputPort
{
    IEnumerable<RelativeMovie> Content { get; }
    Task Handle(IEnumerable<RelativeMovie> data);
}
