namespace DrVideoLibrary.Backend.Repositories.Repositories;
internal class NotificationRepository : INotificationRepository
{
    readonly INotificationContext Context;

    public NotificationRepository(INotificationContext context)
    {
        Context = context;
    }

    public Task DeleteSubscription(string userId, string auth)  =>
        Context.DeleteSubscription(userId, auth);

    public async Task<IEnumerable<NotificationSubscription>> GetSubscriptions()
    {
        IEnumerable<SubscriptionModel> subscriptions = await Context.GetSubscriptions();
        return subscriptions.Select(SubscriptionModel.ToNotificationSubscription);
    }

    public Task NotificationSubscribe(NotificationSubscription subscription) =>
        Context.UpsertSubscribe(subscription);
}
