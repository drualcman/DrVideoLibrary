namespace DrVideoLibrary.Backend.PushNotifications;
internal class NotificationService : INotificationService
{
    readonly NotificationOptions Options;
    readonly INotificationRepository Repository;

    public NotificationService(IOptions<NotificationOptions> options, INotificationRepository repository)
    {
        Options = options.Value;
        Repository = repository;
    }

    public async Task SendNotificationAsync(SendNotificationType type, string message, string link, bool isNew)
    {
        IEnumerable<NotificationSubscription> subscriptions = await Repository.GetSubscriptions();
        if (subscriptions is not null && subscriptions.Any())
        {
            List<Task> tasks = new List<Task>();
            foreach (NotificationSubscription subscription in subscriptions)
            {
                tasks.Add(SendNotification(subscription, type, message, link, isNew));
            }
            await Task.WhenAll(tasks);
        }
    }

    private async Task SendNotification(
        NotificationSubscription subscription, SendNotificationType type, string message, string link, bool isNew)
    {
        using WebPushClient client = new WebPushClient();
        VapidDetails vapidDetails = new VapidDetails($"mailto:{Options.NotificationsEmail}", Options.NotificationsPublicKey, Options.NotificationsPrivateKey);
        PushSubscription pushSubscription = new PushSubscription(subscription.Url, subscription.P256dh, subscription.Auth);
        try
        {
            string payLoad = JsonSerializer.Serialize(new
            {
                message,
                type = type.ToString(),
                url = link,
                update = isNew
            });
            await client.SendNotificationAsync(pushSubscription, payLoad, vapidDetails);
        }
        catch (Exception ex)
        {
            await Repository.DeleteSubscription(subscription.UserId, subscription.Auth);
        }
    }
}
