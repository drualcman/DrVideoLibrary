namespace DrVideoLibrary.Razor.Services;
public class ApiClient
{
    readonly HttpClient Client;
    readonly ApiClientOptions Options;

    public ApiClient(HttpClient client, IOptions<ApiClientOptions> options)
    {
        Client = client;
        Client.BaseAddress = new Uri(options.Value.Url);
        Options = options.Value;
    }

    public async Task<IEnumerable<RelativeMovie>> GetRelativesAsync(RelativesDto data)
    {
        using HttpResponseMessage response = await Client.PostAsJsonAsync($"{Options.Relatives}", data);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<RelativeMovie>>();
    } 

    public async Task<IEnumerable<ListCard>> GetMovies()
    {
        using HttpResponseMessage response = await Client.GetAsync($"{Options.Lists}/all");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<ListCard>>();
    } 

    public async Task<IEnumerable<WatchedCard>> GetWatchedListAsync()
    {
        using HttpResponseMessage response = await Client.GetAsync($"{Options.Lists}/watched");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<WatchedCard>>();
    } 

    public async Task<Movie> GetMovieDetailsAsync(string id)
    {
        using HttpResponseMessage response = await Client.GetAsync($"{Options.Movie}/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Movie>();
    } 

    public async Task AddMovieAsync(Movie data)
    {
        using HttpResponseMessage response = await Client.PostAsJsonAsync($"{Options.Movie}", data);
        response.EnsureSuccessStatusCode();
    }

    public async Task<WatchingNow> GetWatchingNowAsync()
    {
        using HttpResponseMessage response = await Client.GetAsync($"{Options.Watching}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<WatchingNow>();
    }

    public async Task RegisterWatchingNowAsync(string id)
    {
        WatchingNowDto data = new WatchingNowDto
        {
            MovieId = id,
            Start = DateTime.UtcNow,
        };
        using HttpResponseMessage response = await Client.PostAsJsonAsync($"{Options.Watching}", data);
        response.EnsureSuccessStatusCode();
    }
}
