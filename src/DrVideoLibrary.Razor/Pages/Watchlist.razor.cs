namespace DrVideoLibrary.Razor.Pages;
public partial class Watchlist
{
    [Inject] WatchlistViewModel ViewModel { get; set; }
    [Inject] IStringLocalizer<ResourceWatchlist> Localizer { get; set; }
    [Inject] IStringLocalizer<ResourceTimeSpanResult> TimeSpanLocalizer { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await ViewModel.GetList();
    }

    async Task ToPage(int page)
    {
        await InvokeAsync(StateHasChanged);
    }
}
