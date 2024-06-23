namespace DrVideoLibrary.Razor.Pages;
public partial class Index
{
    [Inject] IndexViewModel ViewModel { get; set; }
    [Inject] NavigationManager Navigation { get; set; }
    [Inject] IStringLocalizer<ResourceIndex> Localizer { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await ViewModel.GetList();
    }

    async Task ToPage(int page)
    {
        await InvokeAsync(StateHasChanged);
    }

    public async Task StartPlayMovie(string movieId)
    {                       
        await ViewModel.StartPlayMovie(movieId);
        Navigation.NavigateTo("watching");
    }
}