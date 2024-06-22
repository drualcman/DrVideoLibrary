namespace DrVideoLibrary.Backend.Repositories.Entities;
public class SubscriptionModel
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string Url { get; set; }
    public string P256dh { get; set; }
    public string Auth { get; set; }

    public static NotificationSubscription ToNotificationSubscription(SubscriptionModel model) =>
        new NotificationSubscription
        {
            UserId = model.UserId,
            Url = model.Url,
            P256dh = model.P256dh,
            Auth = model.Auth
        }; 

    public static implicit operator SubscriptionModel (NotificationSubscription model) =>
        new SubscriptionModel
        {
            Id = Guid.NewGuid().ToString(),
            UserId = model.UserId,
            Url = model.Url,
            P256dh = model.P256dh,
            Auth = model.Auth
        };
}
