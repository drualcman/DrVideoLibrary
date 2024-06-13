namespace DrVideoLibrary.Razor.Components;
public partial class ListCardComponent
{
    [Inject] IStringLocalizer<ResourceListCard> Localizer { get; set; }
    [Parameter] public ListCard Movie { get; set; }
}