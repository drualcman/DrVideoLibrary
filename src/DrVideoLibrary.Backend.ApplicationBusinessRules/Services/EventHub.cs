namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Services;
internal class EventHub<TEvent> : IEventHub<TEvent> where TEvent : IEvent
{
    readonly IServiceProvider ServiceProvider;
    readonly ILogger<EventHub<TEvent>> Logger;

    public EventHub(IServiceProvider serviceProvider, ILogger<EventHub<TEvent>> logger)
    {
        ServiceProvider = serviceProvider;
        Logger = logger;
    }

    public void Rise(TEvent data)
    {
        Logger.LogInformation("EventHub.Rise Start");
        Task.Run(async () =>
        {
            Logger.LogInformation("EventHub.Rise Task");
            try
            {
                using AsyncServiceScope scope = ServiceProvider.CreateAsyncScope();
                IEnumerable<IEventHandler<TEvent>> events = scope.ServiceProvider.GetServices<IEventHandler<TEvent>>();
                IEnumerable<Task> tasks = events.Select(e => e.Handle(data)).ToList();
                Logger.LogInformation($"EventHub.Rise Fire #{tasks?.Count()} tasks");
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "EventHub.Rise exception");
                string e = ex.ToString();
                throw;
            }
        });
        Logger.LogInformation("EventHub.Rise End");
    }
}
