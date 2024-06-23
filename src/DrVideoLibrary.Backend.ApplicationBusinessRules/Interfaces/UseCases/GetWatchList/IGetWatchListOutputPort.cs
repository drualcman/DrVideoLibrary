namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.GetWatchList;
public interface IGetWatchListOutputPort
{
    IEnumerable<WatchedCard> Content { get; }
    Task Handle(IEnumerable<WatchedCard> data);
}
