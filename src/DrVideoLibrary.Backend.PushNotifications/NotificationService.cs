using Microsoft.Extensions.Logging;

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

    public async Task SendNotificationAsync(SendNotificationType type, string message, string link, ILogger logger)
    {
        IEnumerable<NotificationSubscription> subscriptions = await Repository.GetSubscriptions();
        logger.LogInformation($"SendNotificationAsync to {subscriptions?.Count()}");
        if (subscriptions is not null && subscriptions.Any())
        {
            List<Task> tasks = new List<Task>();
            foreach (NotificationSubscription subscription in subscriptions)
            {
                tasks.Add(SendNotification(subscription, type, message, link, logger));
            }
            await Task.WhenAll(tasks);
        }
    }

    private async Task SendNotification(
        NotificationSubscription subscription, SendNotificationType type, string message, string link,
        ILogger logger)
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
                url = link
            });
            await client.SendNotificationAsync(pushSubscription, payLoad, vapidDetails);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, $"Can't send push notification with message: '{message}'");
            await Repository.DeleteSubscription(subscription.UserId, subscription.Auth);
        }
    }
}
