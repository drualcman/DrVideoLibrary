
namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.SearchActorInfo;
internal class SearchActorInfoInputPort : ISearchActorInfoInputPort
{
    readonly ISearchMovieService<SearchMovieInSpanish> SearchService;

    public SearchActorInfoInputPort(ISearchMovieService<SearchMovieInSpanish> searchService)
    {
        SearchService = searchService;
    }

    public Task<SearchPersonResult> SearchActor(string name) =>
        SearchService.SearchActor(name);
}
