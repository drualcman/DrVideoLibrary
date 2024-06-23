
namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.GetWatchList;
internal class GetWatchListController : IGetWatchListController
{
    readonly IGetWatchListInputPort Input;
    readonly IGetWatchListOutputPort Presenter;

    public GetWatchListController(IGetWatchListInputPort input, IGetWatchListOutputPort presenter)
    {
        Input = input;
        Presenter = presenter;
    }

    public async Task<IEnumerable<WatchedCard>> GetWatchList()
    {
        await Input.Handle();
        return Presenter.Content;
    }
}
