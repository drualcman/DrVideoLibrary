namespace DrVideoLibrary.Razor.Components;
public partial class ListCardComponent
{
    [Inject] IStringLocalizer<ResourceListCard> Localizer { get; set; }
    [Parameter] public ListCard Movie { get; set; }
    [Parameter] public EventCallback<string> OnPlay { get; set; }

    async Task OnPlay_Click()
    {
        if(OnPlay.HasDelegate)
            await OnPlay.InvokeAsync(Movie.Id);
    }
}