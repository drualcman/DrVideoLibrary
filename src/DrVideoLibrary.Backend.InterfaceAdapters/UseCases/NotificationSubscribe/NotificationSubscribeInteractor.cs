
namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.NotificationSubscribe;
internal class NotificationSubscribeInteractor : INotificationSubscribeInputPort
{
    readonly INotificationRepository Repository;

    public NotificationSubscribeInteractor(INotificationRepository repository)
    {
        Repository = repository;
    }

    public Task Handle(NotificationSubscription subscription) =>
        Repository.NotificationSubscribe(subscription);
}
