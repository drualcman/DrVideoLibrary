namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.GetWatchingNow;
internal class GetWatchingNowController : IGetWatchingNowController
{
    readonly IGetWatchingNowInputPort Input;
    readonly IGetWatchingNowOutputPort Presenter;

    public GetWatchingNowController(IGetWatchingNowInputPort input, IGetWatchingNowOutputPort presenter)
    {
        Input = input;
        Presenter = presenter;
    }

    public async Task<WatchingNow> GetWatchingNow()
    {
        await Input.Handle();
        return Presenter.Content;
    }
}
