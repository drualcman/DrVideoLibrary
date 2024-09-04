

namespace DrVideoLibrary.Razor.Components;
public partial class PersonInfoComponent
{
    [Inject] ISearchMovieService<SearchMovieSpanishService> SearchMovieService { get; set; }
    [Parameter] public string ActorName { get; set; }

    [Parameter] public EventCallback OnClose { get; set; }

    SearchPersonResult ActorResult;
    bool Loading = true;

    protected override async Task OnParametersSetAsync()
    {
        Loading = true;
        if (!string.IsNullOrEmpty(ActorName))
        {
            ActorResult = await SearchMovieService.SearchActor(ActorName);
        }
        Loading = false;
    }

    private void ClosePopup()
    {
        OnClose.InvokeAsync();
    }
}