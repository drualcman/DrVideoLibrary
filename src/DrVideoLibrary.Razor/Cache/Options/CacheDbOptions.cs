namespace DrVideoLibrary.Razor.Cache.Options;
public class CacheDbOptions
{
    public const string SectionKey = nameof(CacheDbOptions);
    public Settings ContextSettings { get; set; }
}
