namespace DrVideoLibrary.Razor.Components;
public partial class WatchedCardComponent
{
    [Inject] IStringLocalizer<ResourceWatchedCardComponent> Localizer { get; set; }
    [Parameter] public WatchedCard Movie { get; set; }

}