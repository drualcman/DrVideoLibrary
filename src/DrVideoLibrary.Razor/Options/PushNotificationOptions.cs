namespace DrVideoLibrary.Razor.Options;
public class PushNotificationOptions
{
    public const string SectionKey = nameof(PushNotificationOptions);
    public string Url { get; set; }
    public string Subscribe { get; set; }
    public string SubscriptionKey { get; set; }
    public string RelativePath { get; set; }
    public string FileName { get; set; }
}
