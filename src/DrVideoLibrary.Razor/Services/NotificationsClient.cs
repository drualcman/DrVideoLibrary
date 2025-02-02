namespace DrVideoLibrary.Razor.Services;

public class NotificationsClient : INotificationClient
{
    readonly HttpClient Client;
    readonly PushNotificationOptions Options;

    public NotificationsClient(HttpClient client, IOptions<PushNotificationOptions> options)
    {
        Client = client;
        Options = options.Value;
        Client.BaseAddress = new Uri(Options.Url);
    }

    public async ValueTask SubscribeToNotification(NotificationSubscription subscription)
    {
        HttpResponseMessage response = await Client.PostAsJsonAsync($"{Options.Subscribe}", subscription);
        response.EnsureSuccessStatusCode();
    }
}
