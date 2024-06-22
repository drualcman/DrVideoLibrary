namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.NotificationSubscribe;
public interface INotificationSubscribeController
{
    Task NotificationSubscribe(NotificationSubscription subscription);
}
