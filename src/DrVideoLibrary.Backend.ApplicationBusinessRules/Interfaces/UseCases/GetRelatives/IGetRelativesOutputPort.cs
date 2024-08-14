namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.GetRelatives;
public interface IGetRelativesOutputPort
{
    IEnumerable<MovieRelationDto> Content { get; }
    Task Handle(IEnumerable<MovieRelationDto> data);
}
