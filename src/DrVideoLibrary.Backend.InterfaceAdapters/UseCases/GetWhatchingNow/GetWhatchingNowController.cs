namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.GetWhatchingNow;
internal class GetWhatchingNowController : IGetWhatchingNowController
{
    readonly IGetWhatchingNowInputPort Input;
    readonly IGetWhatchingNowOutputPort Presenter;

    public GetWhatchingNowController(IGetWhatchingNowInputPort input, IGetWhatchingNowOutputPort presenter)
    {
        Input = input;
        Presenter = presenter;
    }

    public async Task<WatchingNow> GetWhatchingNow()
    {
        await Input.Handle();
        return Presenter.Content;
    }
}
