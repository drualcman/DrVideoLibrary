namespace DrVideoLibrary.Razor.Models;
public class TimeSpanResult
{
    public int Years { get; set; }
    public int Months { get; set; }
    public int Days { get; set; }
    public int Hours { get; set; }
    public int Minutes { get; set; }

    public string ToString(IStringLocalizer resourceManager)
    {
        List<string> parts = new List<string>();
        CultureInfo culture = CultureInfo.CurrentCulture;

        if (Years > 0) parts.Add(string.Format(resourceManager.GetString(nameof(ResourceTimeSpanResult.Years)), Years));
        if (Months > 0) parts.Add(string.Format(resourceManager.GetString(nameof(ResourceTimeSpanResult.Months)), Months));
        if (Days > 0) parts.Add(string.Format(resourceManager.GetString(nameof(ResourceTimeSpanResult.Days)), Days));
        if (Hours > 0) parts.Add(string.Format(resourceManager.GetString(nameof(ResourceTimeSpanResult.Hours)), Hours));
        if (Minutes > 0) parts.Add(string.Format(resourceManager.GetString(nameof(ResourceTimeSpanResult.Minutes)), Minutes));

        return string.Join(", ", parts);
    }

    public static TimeSpanResult FromMinutes(int totalMinutes)
    {
        const int minutesInHour = 60;
        const int minutesInDay = minutesInHour * 24;
        const int minutesInMonth = minutesInDay * 30; // Aproximado
        const int minutesInYear = minutesInDay * 365; // Aproximado

        var result = new TimeSpanResult();

        if (totalMinutes >= minutesInYear)
        {
            result.Years = totalMinutes / minutesInYear;
            totalMinutes %= minutesInYear;
        }

        if (totalMinutes >= minutesInMonth)
        {
            result.Months = totalMinutes / minutesInMonth;
            totalMinutes %= minutesInMonth;
        }

        if (totalMinutes >= minutesInDay)
        {
            result.Days = totalMinutes / minutesInDay;
            totalMinutes %= minutesInDay;
        }

        if (totalMinutes >= minutesInHour)
        {
            result.Hours = totalMinutes / minutesInHour;
            totalMinutes %= minutesInHour;
        }

        result.Minutes = totalMinutes;

        return result;
    }
}



