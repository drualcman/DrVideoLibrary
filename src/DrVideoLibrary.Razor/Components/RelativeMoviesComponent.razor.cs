namespace DrVideoLibrary.Razor.Components;
public partial class RelativeMoviesComponent
{
    [Inject] ApiClient ApiClient { get; set; }
    [Parameter] public string MovieId { get; set; }

    IEnumerable<RelativeMovie> Relatives;
    protected override async Task OnParametersSetAsync()
    {
        Relatives = await ApiClient.GetRelativesAsync(MovieId);
    }
}