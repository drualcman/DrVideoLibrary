namespace DrVideoLibrary.Razor.Options;
public class ApiClientOptions
{
    public const string SectionKey = nameof(ApiClientOptions);
    public string Url { get; set; }
    public string Relatives { get; set; }
    public string Movie { get; set; }
    public string Lists { get; set; }
    public string Watching { get; set; }
}
