namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.GetWatchingNow;
public interface IGetWatchingNowController
{
    Task<WatchingNow> GetWatchingNow();
}
