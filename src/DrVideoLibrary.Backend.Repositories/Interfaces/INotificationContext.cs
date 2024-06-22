namespace DrVideoLibrary.Backend.Repositories.Interfaces;
public interface INotificationContext
{
    Task UpsertSubscribe(SubscriptionModel subscription);
    Task<IEnumerable<SubscriptionModel>> GetSubscriptions();
    Task DeleteSubscription(string userId, string auth);
}
