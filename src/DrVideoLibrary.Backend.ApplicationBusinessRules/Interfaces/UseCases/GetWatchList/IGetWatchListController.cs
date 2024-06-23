namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.GetWatchList;
public interface IGetWatchListController
{
    Task<IEnumerable<WatchedCard>> GetWatchList();
}
