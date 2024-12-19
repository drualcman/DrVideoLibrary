using System.Globalization;

namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.RegisterWatchingNow;
internal class RegisterWatchingNowInteractor : IRegisterWatchingNowInputPort
{
    readonly IMoviesRepository Repository;
    readonly IEventHub<SendNotificationSubscription> EventHub;
    readonly IStringLocalizer<EventMessages> Localizer;

    public RegisterWatchingNowInteractor(IMoviesRepository repository,
        IEventHub<SendNotificationSubscription> eventHub,
        IStringLocalizer<EventMessages> localizer)
    {
        Repository = repository;
        EventHub = eventHub;
        Localizer = localizer;
    }

    public async Task Handle(WatchingNowDto data)
    {
        await Repository.RegisterWatchingNow(data);
        if (!string.IsNullOrEmpty(data.Lang))
        {
            CultureInfo.CurrentCulture = new CultureInfo(data.Lang);
            CultureInfo.CurrentUICulture = new CultureInfo(data.Lang);
        }
        else
        {
            CultureInfo.CurrentCulture = new CultureInfo(ResourcesOptions.DefaultLang);
            CultureInfo.CurrentUICulture = new CultureInfo(ResourcesOptions.DefaultLang);
        }
        EventHub.Rise(new SendNotificationSubscription(
            string.Format(Localizer[nameof(EventMessages.WatchingNowTemplate)], data.Start),
            data.MovieId,
            SendNotificationType.WATCHING));
    }
}
