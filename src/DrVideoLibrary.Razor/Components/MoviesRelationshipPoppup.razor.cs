namespace DrVideoLibrary.Razor.Components;
public partial class MoviesRelationshipPoppup
{
    [Parameter] public MovieBasic[] Movies { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }

    private void ClosePopup()
    {
        OnClose.InvokeAsync();
    }
}