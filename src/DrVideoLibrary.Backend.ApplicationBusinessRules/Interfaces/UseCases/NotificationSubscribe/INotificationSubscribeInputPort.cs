namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.NotificationSubscribe;
public interface INotificationSubscribeInputPort
{
    Task Handle(NotificationSubscription subscription);
}
