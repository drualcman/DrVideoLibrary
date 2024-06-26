namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.RegisterWatchingNow;
internal class RegisterWatchingNowInteractor : IRegisterWatchingNowInputPort
{
    readonly IMoviesRepository Repository;
    readonly IEventHub<SendNotificationSubscription> EventHub;

    public RegisterWatchingNowInteractor(IMoviesRepository repository,
        IEventHub<SendNotificationSubscription> eventHub)
    {
        Repository = repository;
        EventHub = eventHub;
    }

    public async Task Handle(WatchingNowDto data, ILogger logger)
    {
        await Repository.RegisterWatchingNow(data);
        EventHub.Rise(new SendNotificationSubscription(
            $"Comencé a las {data.Start} a ver una peli!", 
            data.MovieId, 
            ApplicationBusinessRules.ValueObjects.SendNotificationType.WATCHING), logger);
    }
}
