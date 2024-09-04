namespace DrVideoLibrary.Backend.Repositories.HttpClients;
internal class SearchMovieSpanishService : ISearchMovieService<SearchMovieInSpanish>
{
    private readonly HttpClient Client;
    private readonly SearchMovieSpanishOption Options;

    public SearchMovieSpanishService(HttpClient client, IOptions<SearchMovieSpanishOption> options)
    {
        Options = options.Value;
        Client = client;
        Client.BaseAddress = new Uri(Options.SearchSpanishUrl);
    }

    public async Task<Movie> GetMovieDetails(string id)
    {
        using HttpResponseMessage response = await Client.GetAsync($"movie/{id}?api_key={Options.SearchSpanishApiKey}&language=es-ES&append_to_response=credits");
        response.EnsureSuccessStatusCode();
        TMDbMovieMovieDetail data = await response.Content.ReadFromJsonAsync<TMDbMovieMovieDetail>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Movie result = null;
        if (data != null)
        {
            result = new Movie
            {
                Id = Guid.NewGuid().ToString(),
                Title = data.Title,
                OriginalTitle = data.OriginalTitle,
                Cover = $"https://image.tmdb.org/t/p/w500{data.PosterPath}",
                Year = DateTime.TryParse(data.ReleaseDate, out DateTime d) ? d.Year : DateTime.Today.Year,
                Description = data.Overview,
                Duration = data.Runtime,
                Actors = data.Credits.Cast.Where(c => c.Department == "Acting").Select(c => c.Name).ToList(),
                Directors = data.Credits.Crew.Where(c => c.Job == "Director").Select(c => c.Name).ToList(),
                Categories = data.Genres.Select(g => g.Name).ToList(),
                Rate = GetRating(data.VoteAverage),
            };
        }
        return result;
    }

    private static byte GetRating(decimal rate) =>
        Convert.ToByte(Math.Round(rate), CultureInfo.InvariantCulture);

    public async Task<IEnumerable<SearchMovieResult>> SearchMovies(string text)
    {
        using HttpResponseMessage response = await Client.GetAsync($"search/movie?api_key={Options.SearchSpanishApiKey}&query={Uri.EscapeDataString(text)}&language=es-ES");
        response.EnsureSuccessStatusCode();
        TMDbSearchResult data = await response.Content.ReadFromJsonAsync<TMDbSearchResult>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        IEnumerable<SearchMovieResult> result = null;
        if (data is not null && data.Results != null && data.Results.Any())
        {
            result = data.Results.Select(r => new SearchMovieResult
            {
                Id = r.Id.ToString(),
                Title = r.Title,
                OriginalTitle = r.OriginalTitle,
                ReleaseDate = r.ReleaseDate,
                Cover = $"https://image.tmdb.org/t/p/w500{r.PosterPath}"
            });
        }
        return result;
    }

    public async Task<SearchPersonResult> SearchActor(string text)
    {
        HttpResponseMessage response = await Client.GetAsync($"search/person?api_key={Options.SearchSpanishApiKey}&query={Uri.EscapeDataString(text)}&language=es-ES");
        response.EnsureSuccessStatusCode();
        TMDbSearchActor data = await response.Content.ReadFromJsonAsync<TMDbSearchActor>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        SearchPersonResult result = new SearchPersonResult
        {
            Name = text
        };
        if (data is not null && data.Results != null && data.Results.Any())
        {
            try
            {
                TMDbSearchActorResult match = GetMatchActor(data.Results, text);
                if (match is not null)
                {
                    response = await Client.GetAsync($"person/{match.Id}?api_key={Options.SearchSpanishApiKey}");
                    response.EnsureSuccessStatusCode();
                    TMDbActor actor = await response.Content.ReadFromJsonAsync<TMDbActor>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    result.Name = match.Name;
                    result.Cover = $"https://image.tmdb.org/t/p/w500{actor.Profile}";
                    result.Biography = actor.Biography;
                    result.Birthday = actor.Birthday;
                    result.PlaceOfBirth = actor.PlaceOfBirth;
                    result.WebSite = actor.Homepage;
                }
            }
            catch (Exception ex)
            {
                result = new SearchPersonResult
                {
                    Name = text,
                    Biography = ex.Message
                };
            }
        }
        response.Dispose();
        return result;
    }

    private TMDbSearchActorResult GetMatchActor(IEnumerable<TMDbSearchActorResult> searchResults, string actorName)
    {
        var matchingResults = searchResults
            .Where(r => string.Equals(r.Name, actorName, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(r.OriginalName, actorName, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(r => r.Popularity)
            .FirstOrDefault();

        return matchingResults;
    }


    class TMDbSearchResult
    {
        public List<TMDbMovie> Results { get; set; }
    }

    class TMDbMovie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [JsonPropertyName("original_title")]
        public string OriginalTitle { get; set; }
        [JsonPropertyName("poster_path")]
        public string PosterPath { get; set; }
        [JsonPropertyName("release_date")]
        public string ReleaseDate { get; set; }
        public string Overview { get; set; }
    }
    class TMDbMovieMovieDetail
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [JsonPropertyName("original_title")]
        public string OriginalTitle { get; set; }
        [JsonPropertyName("poster_path")]
        public string PosterPath { get; set; }
        [JsonPropertyName("release_date")]
        public string ReleaseDate { get; set; }
        public string Overview { get; set; }
        public int Runtime { get; set; }
        public List<TMDbGenre> Genres { get; set; }
        public TMDbCredits Credits { get; set; }
        [JsonPropertyName("vote_average")]
        public decimal VoteAverage { get; set; }
    }
    class TMDbGenre
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    class TMDbCredits
    {
        public List<TMDbCast> Cast { get; set; }
        public List<TMDbCrew> Crew { get; set; }
    }

    class TMDbCast
    {
        public string Name { get; set; }
        [JsonPropertyName("known_for_department")]
        public string Department { get; set; }
    }

    class TMDbCrew
    {
        public string Name { get; set; }
        public string Job { get; set; }
    }

    class TMDbSearchActor
    {
        public TMDbSearchActorResult[] Results { get; set; }
    }
    class TMDbSearchActorResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("original_name")]
        public string OriginalName { get; set; }
        public double Popularity { get; set; }
    } 

    class TMDbActor
    {
        public string Biography { get; set; }
        public string Birthday { get; set; }
        public string Homepage { get; set; }
        public string Name { get; set; }

        [JsonPropertyName("place_of_birth")]
        public string PlaceOfBirth { get; set; }
        [JsonPropertyName("profile_path")]
        public string Profile { get; set; }
    }
}
