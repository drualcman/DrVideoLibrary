namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces;
public interface INotificationRepository
{
    Task NotificationSubscribe(NotificationSubscription subscription);
    Task<IEnumerable<NotificationSubscription>> GetSubscriptions();
    Task DeleteSubscription(string userId, string auth);
}
