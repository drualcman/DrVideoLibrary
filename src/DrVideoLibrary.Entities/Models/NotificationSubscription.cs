namespace DrVideoLibrary.Entities.Models
{
    public class NotificationSubscription
    {
        public string UserId { get; set; }
        public string Url { get; set; }
        public string P256dh { get; set; }
        public string Auth { get; set; }
    }
}
