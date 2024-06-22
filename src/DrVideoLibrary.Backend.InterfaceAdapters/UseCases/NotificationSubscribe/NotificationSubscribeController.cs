
namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.NotificationSubscribe;
internal class NotificationSubscribeController : INotificationSubscribeController
{
    readonly INotificationSubscribeInputPort Input;

    public NotificationSubscribeController(INotificationSubscribeInputPort input)
    {
        Input = input;
    }

    public Task NotificationSubscribe(NotificationSubscription subscription) =>
        Input.Handle(subscription);
}
