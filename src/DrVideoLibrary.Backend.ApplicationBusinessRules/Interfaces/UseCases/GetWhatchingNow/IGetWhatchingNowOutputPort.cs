namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.GetWhatchingNow;
public interface IGetWhatchingNowOutputPort
{
    WatchingNow Content { get; }
    Task Handle(Movie movie, DateTime started);
}
