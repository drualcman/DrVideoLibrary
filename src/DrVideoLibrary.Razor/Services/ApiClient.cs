namespace DrVideoLibrary.Razor.Services;
internal class ApiClient
{
    readonly HttpClient Client;
    readonly ApiClientOptions Options;

    public ApiClient(HttpClient client, IOptions<ApiClientOptions> options)
    {
        Client = client;
        client.BaseAddress = new Uri(options.Value.Url);
        Options = options.Value;
    }

    public async Task<IEnumerable<RelativeMovie>> GetRelativesAsync(string id)
    {
        using HttpResponseMessage response = await Client.GetAsync($"{Options.Relatives}/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<RelativeMovie>>();
    } 
    public async Task<Movie> GetMovieDetailsAsync(string id)
    {
        using HttpResponseMessage response = await Client.GetAsync($"{Options.Movies}/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Movie>();
    }
}
