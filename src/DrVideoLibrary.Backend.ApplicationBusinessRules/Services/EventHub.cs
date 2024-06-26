namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Services;
internal class EventHub<TEvent> : IEventHub<TEvent> where TEvent : IEvent
{
    readonly IServiceProvider ServiceProvider;

    public EventHub(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public void Rise(TEvent data, ILogger logger)
    {
        logger.LogInformation("EventHub.Rise Start");
        Task.Run(async () =>
        {
            logger.LogInformation("EventHub.Rise Task");
            try
            {
                using AsyncServiceScope scope = ServiceProvider.CreateAsyncScope();
                IEnumerable<IEventHandler<TEvent>> events = scope.ServiceProvider.GetServices<IEventHandler<TEvent>>();
                IEnumerable<Task> tasks = events.Select(e => e.Handle(data, logger)).ToList();
                logger.LogInformation($"EventHub.Rise Fire #{tasks?.Count()} tasks");
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "EventHub.Rise exception");
                string e = ex.ToString();
                throw;
            }
        });
        logger.LogInformation("EventHub.Rise End");
    }
}
