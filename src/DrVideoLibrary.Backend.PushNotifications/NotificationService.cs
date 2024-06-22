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

    public async Task SendNotificationAsync(string message, string link)
    {
        IEnumerable<NotificationSubscription> subscriptions = await Repository.GetSubscriptions();
        if (subscriptions is not null && subscriptions.Any())
        {
            List<Task> tasks = new List<Task>();
            foreach (NotificationSubscription subscription in subscriptions)
            {
                tasks.Add(SendNotification(subscription, message, link));
            }
            await Task.WhenAll(tasks);
        }
    }

    private async Task SendNotification(
        NotificationSubscription subscription, string message, string link)
    {
        using WebPushClient client = new WebPushClient();
        VapidDetails vapidDetails = new VapidDetails($"mailto:{Options.NotificationsEmail}", Options.NotificationsPublicKey, Options.NotificationsPrivateKey);
        PushSubscription pushSubscription = new PushSubscription(subscription.Url, subscription.P256dh, subscription.Auth);
        try
        {
            string payLoad = JsonSerializer.Serialize(new
            {
                message,
                url = link
            });
            await client.SendNotificationAsync(pushSubscription, payLoad, vapidDetails);
        }
        catch
        {
            await Repository.DeleteSubscription(subscription.UserId, subscription.Auth);
        }
    }
}
