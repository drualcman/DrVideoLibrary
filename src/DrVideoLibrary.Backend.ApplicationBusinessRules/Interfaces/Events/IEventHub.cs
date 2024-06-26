namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.Events;
public interface IEventHub<TEvent> where TEvent : IEvent
{
    Task Rise(TEvent data, ILogger log);
}
