namespace DrVideoLibrary.Cosmos.DbContext.Helpers;
internal class LowerCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        return name.ToLower();
    }
}
