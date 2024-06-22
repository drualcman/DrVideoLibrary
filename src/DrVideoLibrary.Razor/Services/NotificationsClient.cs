namespace DrVideoLibrary.Razor.Services;

public class NotificationsClient : INotificationClient
{
    readonly HttpClient Client;
    readonly PushNotificationOptions Options;

    public NotificationsClient(HttpClient client, IOptions<PushNotificationOptions> options)
    {
        Client = client;
        Client.BaseAddress = new Uri(options.Value.Url);
        Options = options.Value;
    }

    public async ValueTask SubscribeToNotification(NotificationSubscription subscription)
    {
        HttpResponseMessage response = await Client.PostAsJsonAsync($"{Options.Subscribe}", subscription);
        response.EnsureSuccessStatusCode();
    }
}
