namespace DrVideoLibrary.Razor.Components;
public partial class SelectLangComponent
{

    [Inject] IJSRuntime JS { get; set; }
    [Inject] NavigationManager Navigation { get; set; }

    private Dictionary<string, (string Flag, string Language)> Options = new()
    {
        { "es-ES", ("https://flagcdn.com/es.svg", "Español (España)") },
        { "en-GB", ("https://flagcdn.com/gb.svg", "English (United Kingdom)") },
        //{ "de-DE", ("https://flagcdn.com/de.svg", "Deutsch (Deutschland)") },
        { "ca-ES", ("https://upload.wikimedia.org/wikipedia/commons/thumb/c/ce/Flag_of_Catalonia.svg/1200px-Flag_of_Catalonia.svg.png", "Català (Catalunya)") }
    };

    private bool ShowOptions = false;
    private string SelectedFlag = "https://flagcdn.com/es.svg";
    private string SelectedLanguage = "Español (España)";
#nullable enable
    private CultureInfo? SelectedCulture;

    private CultureInfo[] SupportedCultures = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("es-ES"),
        //new CultureInfo("de-DE"),
        new CultureInfo("ca-ES"),
    };

    protected override void OnInitialized()
    {
        SelectedCulture = CultureInfo.CurrentCulture;
        if (Options.TryGetValue(SelectedCulture.Name, out var option))
        {
            SelectedFlag = option.Flag;
            SelectedLanguage = option.Language;
        }
    }

    private void ToggleOptions()
    {
        ShowOptions = !ShowOptions;
    }

    private async Task SelectOption(string value, string flagSrc, string text)
    {
        SelectedCulture = new CultureInfo(value);
        SelectedFlag = flagSrc;
        SelectedLanguage = text;
        ShowOptions = false;

        if (CultureInfo.CurrentCulture != SelectedCulture)
        {
            await JS.InvokeVoidAsync("blazorCulture.set", SelectedCulture!.Name);
            Navigation.NavigateTo(Navigation.Uri, forceLoad: true);
        }
    }
}