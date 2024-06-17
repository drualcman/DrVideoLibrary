
namespace DrVideoLibrary.Razor.Pages;
public partial class Watching
{
    [Inject] ApiClient Client { get; set; }

    public WatchingNow WatchingNow;

    protected override async Task OnInitializedAsync()
    {
		try
		{
			WatchingNow = await Client.GetWatchingNowAsync();
		}
		catch (Exception ex)
		{
			await Console.Out.WriteLineAsync(ex.ToString());
			WatchingNow = new() { Movie = new() { Actors = [], Directors = [], Categories = ["Not found"], Title = "Not Found", Id = "", Year = DateTime.Today.Year, Description = ex.Message } };
		}
    }
}