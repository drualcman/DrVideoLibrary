namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.GetAll;
public interface IGetAllOutputPort
{
    IEnumerable<ListCard> Content { get; }
    Task Handle(IEnumerable<ListCard> data);
}
