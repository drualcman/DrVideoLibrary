namespace DrVideoLibrary.Razor.Pages;
public partial class MovieDetail
{
    [Inject] ApiClient Client { get; set; }
    [Inject] NavigationManager Navigation { get; set; }
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
        await Client.RegisterWatchingNowAsync(Id);
        Navigation.NavigateTo("watching");
    }

}