
namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.GetMovieById;
internal class GetMovieByIdController : IGetMovieByIdController
{
    readonly IGetMovieByIdInputPort InputPort;
    readonly IGetMovieByIdOutputPort Presenter;

    public GetMovieByIdController(IGetMovieByIdInputPort inputPort, IGetMovieByIdOutputPort presenter)
    {
        InputPort = inputPort;
        Presenter = presenter;
    }

    public async Task<Movie> GetMovieById(string id)
    {
        await InputPort.Handle(id);
        return Presenter.Content;
    }
}
