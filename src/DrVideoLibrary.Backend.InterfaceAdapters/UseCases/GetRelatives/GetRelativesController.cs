namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.GetRelatives;
internal class GetRelativesController : IGetRelativesController
{
    readonly IGetRelativesInputPort Input;
    readonly IGetRelativesOutputPort Presenter;

    public GetRelativesController(IGetRelativesInputPort input, IGetRelativesOutputPort presenter)
    {
        Input = input;
        Presenter = presenter;
    }

    public async Task<IEnumerable<RelativeMovie>> GetRelatives(string id)
    {
        await Input.Handle(id);
        return Presenter.Content;
    }
}
