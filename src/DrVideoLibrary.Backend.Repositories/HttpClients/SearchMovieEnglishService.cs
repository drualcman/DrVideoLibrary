namespace DrVideoLibrary.Backend.Repositories.HttpClients;
internal class SearchMovieEnglishService : ISearchMovieService<SearchMovieInEnglish>
{
    private readonly HttpClient Client;
    private readonly SearchMovieEnglishOption Options;
    private readonly ITranslateService TranslationService;

    public SearchMovieEnglishService(HttpClient client, IOptions<SearchMovieEnglishOption> options, ITranslateService translationService)
    {
        Options = options.Value;
        Client = client;
        Client.BaseAddress = new Uri(Options.SearchEnglishUrl);
        TranslationService = translationService;
    }

    public async Task<Movie> GetMovieDetails(string id)
    {
        using HttpResponseMessage response = await Client.GetAsync($"?i={id}&apikey={Options.SearchEnglishApiKey}");
        response.EnsureSuccessStatusCode();
        OmdbMovieDetail data = await response.Content.ReadFromJsonAsync<OmdbMovieDetail>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Movie result = null;
        if (data != null)
        {
            string translatedText = await TranslationService.TranslateTextAsync(data.Plot, "EN", "ES"); // Translate to Espanish
            result = new Movie
            {
                Id = Guid.NewGuid().ToString(),
                Title = data.Title,
                OriginalTitle = data.Title,
                Cover = data.Poster,
                Year = int.TryParse(data.Year, out int y) ? y : DateTime.Now.Year,
                Description = translatedText,
                Duration = ParseRuntime(data.Runtime),
                Directors = data.Director?.Split(',').Select(d => d.Trim()).ToList(),
                Actors = data.Actors?.Split(',').Select(a => a.Trim()).ToList(),
                Categories = data.Genre?.Split(',').Select(g => g.Trim()).ToList(),
                Rate = GetRating(data.ImdbRating),
            };
        }
        return result;
    }

    public Task<SearchPersonResult> SearchActor(string text) => Task.FromResult(new SearchPersonResult { Name = text });
 
    public async Task<IEnumerable<SearchMovieResult>> SearchMovies(string text)
    {
        using HttpResponseMessage response = await Client.GetAsync($"?s={Uri.EscapeDataString(text)}&apikey={Options.SearchEnglishApiKey}");
        response.EnsureSuccessStatusCode();
        OmdbSearchResult data = await response.Content.ReadFromJsonAsync<OmdbSearchResult>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        IEnumerable<SearchMovieResult> result = null;
        if (data is not null && data.Search is not null)
        {
            result = data.Search.Select(r => new SearchMovieResult
            {
                Id = r.imdbID,
                Title = r.Title,
                OriginalTitle = r.Title,
                ReleaseDate = r.Year,
                Cover = ""
            });
        }
        return result;
    }

    private static byte GetRating(string rate) => string.IsNullOrEmpty(rate)
        ? (byte)0
        : Convert.ToByte(Math.Round(Convert.ToDouble(rate, CultureInfo.InvariantCulture)));


    private int ParseRuntime(string runtime)
    {
        if (int.TryParse(runtime?.Split(' ')[0], out int minutes))
        {
            return minutes;
        }
        return 0;
    }

    class OmdbSearchResult
    {
        public List<OmdbMovie> Search { get; set; }
    }

    class OmdbMovie
    {
        public string imdbID { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
    }

    class OmdbMovieDetail
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string Released { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Actors { get; set; }
        public string Plot { get; set; }
        public string Poster { get; set; }
        public string ImdbRating { get; set; }
    }
}
