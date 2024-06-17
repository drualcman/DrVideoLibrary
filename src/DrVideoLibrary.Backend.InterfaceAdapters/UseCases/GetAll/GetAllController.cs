
namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.GetAll;
internal class GetAllController : IGetAllController
{
    readonly IGetAllInputPort Input;
    readonly IGetAllOutputPort Presenter;

    public GetAllController(IGetAllInputPort input, IGetAllOutputPort output)
    {
        Input = input;
        Presenter = output;
    }

    public async Task<IEnumerable<ListCard>> GetAll()
    {
        await Input.Handle();
        return Presenter.Content;
    }
}
