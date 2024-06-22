namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Models;
public class SendNotificationSubscription : IEvent
{
    public string Message { get; set; }
    public string MovieId { get; set; }
    public SendNotificationType NotificationType { get; set; }

    public SendNotificationSubscription(string message, string movieId, SendNotificationType notificationType)
    {
        Message = message;
        MovieId = movieId;
        NotificationType = notificationType;
    }
}
