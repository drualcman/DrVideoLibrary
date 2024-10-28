namespace DrVideoLibrary.Functions.Helpers;

public static class HttpRequestHelper
{
    public static async Task<TValue> GetRequestedModel<TValue>(HttpRequest req)
    {
        string body = await ReadAsStringAsync(req);
        TValue data = JsonSerializer.Deserialize<TValue>(body, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        return data;
    }

    private static async Task<string> ReadAsStringAsync(HttpRequest request)
    {
        using StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, 1024, leaveOpen: true);
        string result = await reader.ReadToEndAsync();
        request.Body.Seek(0L, SeekOrigin.Begin);
        return result;
    }
}