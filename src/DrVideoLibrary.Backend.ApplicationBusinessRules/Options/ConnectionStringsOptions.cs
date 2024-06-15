namespace DrVideoLibrary.Backend.ApplicationBusinessRules.Options;
public class ConnectionStringsOptions
{
    public const string SectionKey = "ConnectionStrings";
    public string Database { get; set; }
    public string Storage { get; set; }
}
