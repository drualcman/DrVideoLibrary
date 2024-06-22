namespace DrVideoLibrary.Backend.Repositories.Interfaces;
public interface INotificationContext
{
    Task UpsertSubscribe(SubscriptionModel subscription);
    Task<IEnumerable<SubscriptionModel>> GetSubscriptions();
}
