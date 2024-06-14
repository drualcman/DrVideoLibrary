namespace DrVideoLibrary.Razor.Components;
public partial class ProgressBarComponent : IDisposable
{
    [Parameter] public DateTime Start { get; set; }
    [Parameter] public int TotalMinutes { get; set; }
    private string ProgressWidth { get; set; }
    private string DisplayTime { get; set; }
    private string TotalTime { get; set; }
    private Timer timer;

    protected override void OnInitialized()
    {
        timer = new Timer(UpdateProgress, null, 0, 5000);
        TotalTime = TimeSpan.FromMinutes(TotalMinutes).ToString(@"hh\:mm\:ss");
    }

    private void UpdateProgress(object state)
    {
        var elapsed = DateTime.Now - Start;
        var elapsedMinutes = elapsed.TotalMinutes;
        ProgressWidth = Math.Min((elapsedMinutes / TotalMinutes) * 100, 100).ToString("F2", CultureInfo.InvariantCulture);
        DisplayTime = $"{elapsed.Hours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}";
        InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        timer?.Dispose();
    }
}