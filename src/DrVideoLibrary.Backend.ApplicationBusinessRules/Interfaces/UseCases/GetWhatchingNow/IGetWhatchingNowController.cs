namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.GetWhatchingNow;
public interface IGetWhatchingNowController
{
    Task<WatchingNow> GetWhatchingNow();
}
