namespace DrVideoLibrary.Razor.Pages;
public partial class MovieDetail
{
    [Inject] ApiClient Client { get; set; }
    [Inject] NavigationManager Navigation { get; set; }
    [Inject] IJSRuntime JSRuntime { get; set; }
    [Parameter] public string Id { get; set; }
    Movie Movie;

    protected override void OnParametersSet()
    {
        Movie = null;
    }

    protected override async Task OnParametersSetAsync()
    {
        Movie = await Client.GetMovieDetailsAsync(Id);
    }

    public async Task StartPlayMovie()
    {
        string lang =  await JSRuntime.InvokeAsync<string>("blazorCulture.get");
        await Client.RegisterWatchingNowAsync(Id, lang);
        Navigation.NavigateTo("watching");
    }

}