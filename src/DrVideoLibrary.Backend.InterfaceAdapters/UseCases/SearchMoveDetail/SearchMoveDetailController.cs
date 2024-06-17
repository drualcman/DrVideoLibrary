namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.SearchMoveDetail;
internal class SearchMoveDetailController : ISearchMoveDetailController
{
    readonly ISearchMoveDetailInputPort Input;

    public SearchMoveDetailController(ISearchMoveDetailInputPort input)
    {
        Input = input;
    }

    public Task<Movie> SearchMoveDetail(string id, string lang)  =>
        Input.SearchMoveDetail(id, lang);
}
