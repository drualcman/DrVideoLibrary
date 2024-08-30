namespace DrVideoLibrary.Razor.Shared;
public partial class MainLayout
{
    [Inject] MoviesCacheService CacheService { get; set; }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            CacheService.ProcessRelatives();
        }
    }
}