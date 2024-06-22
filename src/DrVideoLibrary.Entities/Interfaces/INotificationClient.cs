namespace DrVideoLibrary.Entities.Interfaces;

public interface INotificationClient
{
    ValueTask SubscribeToNotification(NotificationSubscription subscription);
}
