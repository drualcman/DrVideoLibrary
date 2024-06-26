namespace DrVideoLibrary.Backend.InterfaceAdapters.Events;
internal class WatchingMovieHandler : IEventHandler<SendNotificationSubscription>
{
    readonly INotificationService NotificationService;
    readonly IUrlProvider UrlProvider;
    readonly ILogger Logger;

    public WatchingMovieHandler(INotificationService notificationService, 
        IUrlProvider urlProvider, ILogger logger)
    {
        NotificationService = notificationService;
        UrlProvider = urlProvider;
        Logger = logger;
    }

    public async Task Handle(SendNotificationSubscription data)
    {
        Logger.LogInformation($"WatchingMovieHandler Event Rise type {data.NotificationType}");
        if (data.NotificationType == ApplicationBusinessRules.ValueObjects.SendNotificationType.WATCHING)
        {
            Logger.LogInformation("WatchingMovieHandler Event Rise WATCHING");
            await NotificationService.SendNotificationAsync(data.NotificationType, data.Message, UrlProvider.GetUrl("watching"));
        }
    }
}
