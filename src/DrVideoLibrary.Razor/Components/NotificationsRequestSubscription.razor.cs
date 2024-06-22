namespace DrVideoLibrary.Razor.Components;

public partial class NotificationsRequestSubscription : ComponentBase, IAsyncDisposable
{
    [Inject] IOptions<PushNotificationOptions> Configuration { get; set; }
    [Inject] IJSRuntime JsRuntime { get; set; }
    [Inject] INotificationClient Notification { get; set; }
    [Inject] IStringLocalizer<ResourceNotificationsRequestSubscription> Localizer { get; set; }

    bool HasNotGrandNotifications;
    IJSObjectReference PageScripts;
    PushNotificationOptions Options;

    protected override void OnInitialized()
    {
        Options = Configuration.Value;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            PageScripts = await JsRuntime.ImportJavascript($"Components/{nameof(NotificationsRequestSubscription)}.razor.js", typeof(NotificationsRequestSubscription).Assembly);
            HasNotGrandNotifications = await PageScripts.InvokeAsync<bool>("hasNotGrandNotifications");
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task RequestNotificationPermission()
    {
        try
        {
            NotificationSubscription subscription = await PageScripts.InvokeAsync<NotificationSubscription>("setupAndSubscribe",
                Options.SubscriptionKey, Options.RelativePath, Options.FileName);
            if (subscription is not null)
            {
                subscription.UserId = await GetFingerPrint(PageScripts);
                await Notification.SubscribeToNotification(subscription);
                HasNotGrandNotifications = false;
            }
            else
                HasNotGrandNotifications = await PageScripts.InvokeAsync<bool>("hasNotGrandNotifications");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public async ValueTask<string> GetFingerPrint(IJSObjectReference module)
    {
        string data = await module.InvokeAsync<string>("getFingerPrint");
        using System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create();
        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
        StringBuilder builder = new StringBuilder();
        foreach (byte b in bytes)
        {
            builder.Append(b.ToString("x2"));
        }
        return builder.ToString();
    }

    public async ValueTask DisposeAsync()
    {
        await PageScripts.DisposeAsync();
    }
}