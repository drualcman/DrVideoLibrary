namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.RegisterWatchingNow;
internal class RegisterWatchingNowInteractor : IRegisterWatchingNowInputPort
{
    readonly IMoviesRepository Repository;
    readonly IEventHub<SendNotificationSubscription> EventHub;
    readonly ILogger<RegisterWatchingNowInteractor> Logger;

    public RegisterWatchingNowInteractor(IMoviesRepository repository,
        IEventHub<SendNotificationSubscription> eventHub,
        ILogger<RegisterWatchingNowInteractor> logger)
    {
        Repository = repository;
        EventHub = eventHub;
        Logger = logger;
    }

    public async Task Handle(WatchingNowDto data)
    {
        Logger.LogInformation($"RegisterWatchingNowInteractor.Handle #{data.MovieId}");
        await Repository.RegisterWatchingNow(data);
        await EventHub.Rise(new SendNotificationSubscription(
            $"Comence a las {data.Start} a ver una peli!", 
            data.MovieId, 
            ApplicationBusinessRules.ValueObjects.SendNotificationType.WATCHING));
    }
}
