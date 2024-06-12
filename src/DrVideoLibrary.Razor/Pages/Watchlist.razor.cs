namespace DrVideoLibrary.Razor.Pages;
public partial class Watchlist
{
    [Inject] WatchlistViewModel ViewModel { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await ViewModel.GetList();
    }
}
