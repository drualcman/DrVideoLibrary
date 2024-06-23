namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.GetWatchingNow;
public interface IGetWatchingNowOutputPort
{
    WatchingNow Content { get; }
    Task Handle(Movie movie, DateTime started);
}
