
namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.RegisterWatchingNow;
internal class RegisterWatchingNowInteractor : IRegisterWatchingNowInputPort
{
    readonly IMoviesRepository Repository;

    public RegisterWatchingNowInteractor(IMoviesRepository repository)
    {
        Repository = repository;
    }

    public async Task Handle(string id) =>
        await Repository.RegisterWatchingNow(id);
}
