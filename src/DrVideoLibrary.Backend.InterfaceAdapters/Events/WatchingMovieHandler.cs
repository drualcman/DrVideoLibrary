namespace DrVideoLibrary.Backend.InterfaceAdapters.Events;
internal class WatchingMovieHandler : IEventHandler<SendNotificationSubscription>
{
    readonly INotificationService NotificationService;
    readonly IUrlProvider UrlProvider;

    public WatchingMovieHandler(INotificationService notificationService, 
        IUrlProvider urlProvider)
    {
        NotificationService = notificationService;
        UrlProvider = urlProvider;
    }

    public async Task Handle(SendNotificationSubscription data, ILogger logger)
    {
        logger.LogInformation($"WatchingMovieHandler Event Rise type {data.NotificationType}");
        if (data.NotificationType == ApplicationBusinessRules.ValueObjects.SendNotificationType.WATCHING)
        {
            logger.LogInformation("WatchingMovieHandler Event Rise WATCHING");
            await NotificationService.SendNotificationAsync(data.NotificationType, data.Message, UrlProvider.GetUrl("watching"), logger);
        }
    }
}
