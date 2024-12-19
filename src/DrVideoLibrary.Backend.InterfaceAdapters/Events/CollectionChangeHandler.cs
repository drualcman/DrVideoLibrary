namespace DrVideoLibrary.Backend.InterfaceAdapters.Events;
internal class CollectionChangeHandler : IEventHandler<SendNotificationSubscription>
{
    readonly INotificationService NotificationService;
    readonly IUrlProvider UrlProvider;

    public CollectionChangeHandler(INotificationService notificationService, IUrlProvider urlProvider)
    {
        NotificationService = notificationService;
        UrlProvider = urlProvider;
    }

    public async Task Handle(SendNotificationSubscription data)
    {
        if (data.NotificationType == SendNotificationType.CATALOG)
        {
            await NotificationService.SendNotificationAsync(data.NotificationType, data.Message, UrlProvider.GetUrl(""), false);
        }
    }
}
