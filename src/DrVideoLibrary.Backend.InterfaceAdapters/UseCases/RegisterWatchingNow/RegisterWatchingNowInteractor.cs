
namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.RegisterWatchingNow;
internal class RegisterWatchingNowInteractor : IRegisterWatchingNowInputPort
{
    readonly IMoviesRepository Repository;

    public RegisterWatchingNowInteractor(IMoviesRepository repository)
    {
        Repository = repository;
    }

    public async Task Handle(WatchingNowDto data) =>
        await Repository.RegisterWatchingNow(data);
}
