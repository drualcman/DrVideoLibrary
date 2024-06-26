namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.Events;
public interface IEventHub<TEvent> where TEvent : IEvent
{
    void Rise(TEvent data, ILogger log);
}
