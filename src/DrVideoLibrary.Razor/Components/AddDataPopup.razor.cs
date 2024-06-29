namespace DrVideoLibrary.Razor.Components;
public partial class AddDataPopup
{
    [Parameter] public EventCallback<string> OnAdd { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }

    string ToAdd;
    bool IsShowing;

    async Task Add()
    {
        IsShowing = false;
        if (OnAdd.HasDelegate)
            await OnAdd.InvokeAsync(ToAdd);
        ToAdd = string.Empty;
    }

    async Task Cancel()
    {
        IsShowing = false;
        if(OnCancel.HasDelegate)
            await OnCancel.InvokeAsync();
        ToAdd = string.Empty;
    }

}