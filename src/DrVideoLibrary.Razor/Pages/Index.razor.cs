namespace DrVideoLibrary.Razor.Pages;
public partial class Index
{
    [Inject] IndexViewModel ViewModel { get; set; }
    [Inject] NavigationManager Navigation { get; set; }
    [Inject] IStringLocalizer<ResourceIndex> Localizer { get; set; }
    [Inject] IJSRuntime JSRuntime { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await ViewModel.GetList();
            await InvokeAsync(StateHasChanged);
        }
    }

    async Task ToPage(int page)
    {
        await InvokeAsync(StateHasChanged);
    }

    public async Task StartPlayMovie(string movieId)
    {
        string lang = await JSRuntime.InvokeAsync<string>("blazorCulture.get");
        await ViewModel.StartPlayMovie(movieId, lang);
        Navigation.NavigateTo("watching");
    }
}