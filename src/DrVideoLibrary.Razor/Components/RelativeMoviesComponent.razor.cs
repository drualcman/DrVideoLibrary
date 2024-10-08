namespace DrVideoLibrary.Razor.Components;
public partial class RelativeMoviesComponent
{
    [Inject] MoviesCacheService CacheService { get; set; }
    [Inject] IStringLocalizer<ResourceRelativeMoviesComponent> Localizer { get; set; }
    [Parameter] public string MovieId { get; set; }
    [Parameter] public RelativesDto Data { get; set; }
    [Parameter] public EventCallback<RelativeType> OnChangeRelative { get; set; }

    IEnumerable<RelativeMovie> Relatives;

    protected override void OnParametersSet()
    {
        Relatives = null;
    }

    protected override async Task OnParametersSetAsync()
    {
        await SearchRelatives();
    }

    async Task SearchRelatives()
    {
        List<RelativeMovie> relatives = new(await CacheService.GetRelativesAsync(Data));
        if (!string.IsNullOrEmpty(MovieId))
        {
            RelativeMovie except = relatives.FirstOrDefault(m => m.Id.Equals(MovieId, StringComparison.InvariantCultureIgnoreCase));
            if (except != null)
                relatives.Remove(except);
        }
        Relatives = relatives;
    }

    async Task ChangeRelative(RelativeType relativeType)
    {
        if (Data.RelativeOf != relativeType && OnChangeRelative.HasDelegate)
            await OnChangeRelative.InvokeAsync(relativeType);
    }
}