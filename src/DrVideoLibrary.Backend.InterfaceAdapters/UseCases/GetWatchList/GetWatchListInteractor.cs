
namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.GetWatchList;
internal class GetWatchListInteractor : IGetWatchListInputPort
{
    readonly IGetWatchListOutputPort Output;
    readonly IMoviesRepository Repository;

    public GetWatchListInteractor(IGetWatchListOutputPort output, IMoviesRepository repository)
    {
        Output = output;
        Repository = repository;
    }

    public async Task Handle()
    {
        IEnumerable<WatchedCard> data = await Repository.GetWatchList();
        await Output.Handle(data);
    }
}
