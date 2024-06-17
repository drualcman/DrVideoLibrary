namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.RegisterWatchingNow;
internal class RegisterWatchingNowController : IRegisterWatchingNowController
{
    readonly IRegisterWatchingNowInputPort Input;

    public RegisterWatchingNowController(IRegisterWatchingNowInputPort input)
    {
        Input = input;
    }

    public Task RegisterWatchingNow(WatchingNowDto data)  => 
        Input.Handle(data);
}
