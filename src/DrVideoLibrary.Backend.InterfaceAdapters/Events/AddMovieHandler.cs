namespace DrVideoLibrary.Backend.InterfaceAdapters.Events;
internal class AddMovieHandler : IEventHandler<SendNotificationSubscription>
{
    readonly INotificationService NotificationService;
    readonly IUrlProvider UrlProvider;

    public AddMovieHandler(INotificationService notificationService, IUrlProvider urlProvider)
    {
        NotificationService = notificationService;
        UrlProvider = urlProvider;
    }

    public async Task Handle(SendNotificationSubscription data)
    {
        await NotificationService.SendNotificationAsync(data.Message, UrlProvider.GetUrl($"movie/{data.MovieId}"));
    }
}
