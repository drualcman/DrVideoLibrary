namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.Events;
public interface IEventHandler<TEvent> where TEvent : IEvent
{
    Task Handle(TEvent data, ILogger logger);
}
