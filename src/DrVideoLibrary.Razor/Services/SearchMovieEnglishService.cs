namespace DrVideoLibrary.Razor.Services;
internal class SearchMovieEnglishService : ISearchMovieService<SearchMovieEnglishService>
{
    private readonly HttpClient Client;
    private readonly SearchMovieServiceOption Options;

    public SearchMovieEnglishService(HttpClient client, IOptions<SearchMovieOptions> options)
    {
        Options = options.Value.Services.FirstOrDefault(s => s.Language == SearchMovieLang.EN);
        if (Options is not null)
        {
            Client = client;
            Client.BaseAddress = new Uri(Options.Url);
        }
        else
            throw new ArgumentNullException(nameof(Options));
    }

    public async Task<Movie> GetMovieDetails(string id)
    {
        HttpResponseMessage response = await Client.GetAsync($"{id}?l={Options.Language}");
        response.EnsureSuccessStatusCode();
        Movie result = await response.Content.ReadFromJsonAsync<Movie>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });        
        return result;
    }

    public async Task<IEnumerable<SearchMovieResult>> SearchMovies(string text)
    {
        HttpResponseMessage response = await Client.GetAsync($"?s={text}&l={Options.Language}");
        response.EnsureSuccessStatusCode();
        IEnumerable<SearchMovieResult> result = await response.Content.ReadFromJsonAsync<IEnumerable<SearchMovieResult>>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });        
        return result;
    }
}
