namespace DrVideoLibrary.Razor.Components;
public partial class ProgressBarComponent : IDisposable
{
    [Inject] IStringLocalizer<ProgressBarComponentResource> Localizer { get; set; }
    [Parameter] public DateTime Start { get; set; }
    [Parameter] public int TotalMinutes { get; set; }
    private string ProgressWidth { get; set; }
    private string DisplayTime { get; set; }
    private string TotalTime { get; set; }
    private string Remaing { get; set; }
    private Timer timer;

    protected override void OnInitialized()
    {
        timer = new Timer(UpdateProgress, null, 0, 5000);
        TotalTime = TimeSpan.FromMinutes(TotalMinutes).ToString(@"hh\:mm\:ss");
        Remaing = TimeSpan.FromMinutes(TotalMinutes).ToString();
    }

    private void UpdateProgress(object state)
    {
        TimeSpan elapsed = DateTime.UtcNow - Start;
        double elapsedMinutes = elapsed.TotalMinutes;
        ProgressWidth = Math.Min((elapsedMinutes / TotalMinutes) * 100, 100).ToString("F2", CultureInfo.InvariantCulture);
        if (elapsedMinutes >= TotalMinutes)
        {
            elapsed = Start.AddMinutes(TotalMinutes) - Start;
            timer.Dispose();
            Remaing = "";
        }
        else
            Remaing = $"(-{(int)(TotalMinutes - elapsedMinutes)} {Localizer.GetString(nameof(ProgressBarComponentResource.TotalTimeLabel))})";
        DisplayTime = $"{elapsed.Hours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}";
        InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        timer?.Dispose();
    }
}