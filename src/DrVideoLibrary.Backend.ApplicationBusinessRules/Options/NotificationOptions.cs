namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Options;
public class NotificationOptions
{
    public const string SectionKey = nameof(NotificationOptions);
    public string NotificationsPublicKey { get; set; }
    public string NotificationsPrivateKey { get; set; }
    public string NotificationsEmail { get; set; }
}
