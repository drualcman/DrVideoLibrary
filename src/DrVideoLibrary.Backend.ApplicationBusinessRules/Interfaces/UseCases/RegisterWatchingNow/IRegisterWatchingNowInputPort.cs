namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.RegisterWatchingNow;
public interface IRegisterWatchingNowInputPort
{
    Task Handle(WatchingNowDto id);
}
