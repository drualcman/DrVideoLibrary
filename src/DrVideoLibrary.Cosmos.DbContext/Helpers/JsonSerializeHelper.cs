namespace DrVideoLibrary.Cosmos.DbContext.Helpers;
internal static class JsonSerializeHelper
{

    public static string Parse<TData>(TData data)
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = new LowerCaseNamingPolicy(),
            WriteIndented = false // Opcional, para formato legible
        };
        return JsonSerializer.Serialize(data, options);
    }
}
