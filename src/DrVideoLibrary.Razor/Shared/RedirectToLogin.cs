namespace DrVideoLibrary.Razor.Shared;
public partial class RedirectToLogin : ComponentBase
{
    [Inject] NavigationManager Navigation {  get; set; }
    protected override void OnInitialized()
    {
        Navigation.NavigateToLogin("authentication/login");
    }
}