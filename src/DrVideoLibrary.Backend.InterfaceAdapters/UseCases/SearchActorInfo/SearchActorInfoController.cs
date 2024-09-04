
namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.SearchActorInfo;
internal class SearchActorInfoController : ISearchActorInfoController
{
    private readonly ISearchActorInfoInputPort Input;

    public SearchActorInfoController(ISearchActorInfoInputPort input)
    {
        Input = input;
    }

    public Task<SearchPersonResult> SearchActor(string name) =>
        Input.SearchActor(name);
}
