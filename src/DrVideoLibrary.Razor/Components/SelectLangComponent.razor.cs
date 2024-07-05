namespace DrVideoLibrary.Razor.Components;
public partial class SelectLangComponent
{

    [Inject] IJSRuntime JS { get; set; }
    [Inject] NavigationManager Navigation { get; set; }

    private Dictionary<string, (string Flag, string Language)> options = new()
    {
        { "es-ES", ("https://flagcdn.com/es.svg", "Español (España)") },
        { "en-GB", ("https://flagcdn.com/gb.svg", "English (United Kingdom)") },
        { "de-DE", ("https://flagcdn.com/de.svg", "Deutsch (Deutschland)") },
        { "ca-ES", ("https://upload.wikimedia.org/wikipedia/commons/thumb/c/ce/Flag_of_Catalonia.svg/1200px-Flag_of_Catalonia.svg.png", "Català (Catalunya)") }
    };

    private bool showOptions = false;
    private string selectedFlag = "https://flagcdn.com/es.svg";
    private string selectedLanguage = "Español (España)";
    private CultureInfo? selectedCulture;

    private CultureInfo[] supportedCultures = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("es-ES"),
        new CultureInfo("ca-ES"),
        new CultureInfo("de-DE"),
    };

    protected override void OnInitialized()
    {
        selectedCulture = CultureInfo.CurrentCulture;
        if (options.TryGetValue(selectedCulture.Name, out var option))
        {
            selectedFlag = option.Flag;
            selectedLanguage = option.Language;
        }
    }

    private void ToggleOptions()
    {
        showOptions = !showOptions;
    }

    private async Task SelectOption(string value, string flagSrc, string text)
    {
        selectedCulture = new CultureInfo(value);
        selectedFlag = flagSrc;
        selectedLanguage = text;
        showOptions = false;
 
        if (CultureInfo.CurrentCulture != selectedCulture)
        {
            await JS.InvokeVoidAsync("blazorCulture.set", selectedCulture!.Name);
            Navigation.NavigateTo(Navigation.Uri, forceLoad: true);
        }
    }
}