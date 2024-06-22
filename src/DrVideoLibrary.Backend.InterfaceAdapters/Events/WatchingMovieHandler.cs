﻿namespace DrVideoLibrary.Backend.InterfaceAdapters.Events;
internal class WatchingMovieHandler : IEventHandler<SendNotificationSubscription>
{
    readonly INotificationService NotificationService;
    readonly IUrlProvider UrlProvider;

    public WatchingMovieHandler(INotificationService notificationService, IUrlProvider urlProvider)
    {
        NotificationService = notificationService;
        UrlProvider = urlProvider;
    }

    public async Task Handle(SendNotificationSubscription data)
    {
        if (data.NotificationType == ApplicationBusinessRules.ValueObjects.SendNotificationType.WATCHING)
        {
            await NotificationService.SendNotificationAsync(data.Message, UrlProvider.GetUrl("watching"));
        }
    }
}
