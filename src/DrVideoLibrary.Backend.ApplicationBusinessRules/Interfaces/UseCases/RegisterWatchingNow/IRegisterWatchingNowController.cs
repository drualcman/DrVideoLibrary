namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.RegisterWatchingNow;
public interface IRegisterWatchingNowController
{
    Task RegisterWatchingNow(WatchingNowDto data);
}
