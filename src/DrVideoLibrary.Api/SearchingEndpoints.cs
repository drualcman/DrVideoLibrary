namespace DrVideoLibrary.Api
{
    internal class SearchingEndpoints
    {
        readonly ISearchMovieService<SearchMovieEnglishService> EnglishService;
        readonly ISearchMovieService<SearchMovieSpanishService> SpanishService;
        public SearchingEndpoints(
            ISearchMovieService<SearchMovieEnglishService> englishService,
            ISearchMovieService<SearchMovieSpanishService> spanishService)
        {
            EnglishService = englishService;
            SpanishService = spanishService;
        }

        [FunctionName("GetMoviesFromTitle")]
        public async Task<IActionResult> GetMoviesFromTitle(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "search")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Get movie list from title");

            try
            {
                string text = req.Query["s"];
                string lang = req.Query["l"];
                IEnumerable<SearchMovieResult> data;
                switch (lang.ToLower())
                {
                    case "en":
                        data = await EnglishService.SearchMovies(text);
                        break;
                    case "es":
                        data = await SpanishService.SearchMovies(text);
                        break;
                    default:
                        data = new List<SearchMovieResult>();
                        break;
                };
                return new OkObjectResult(data);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message).ToProblemDetails();
            }
        }

        [FunctionName("GetMovieDetail")]
        public async Task<IActionResult> GetMovieDetail(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "search/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            log.LogInformation("Movie details from id");

            try
            {
                string lang = req.Query["l"];
                Movie movie;
                switch (lang.ToLower())
                {
                    case "en":
                        movie = await EnglishService.GetMovieDetails(id);
                        break;
                    case "es":
                        movie = await SpanishService.GetMovieDetails(id);
                        break;
                    default:
                        movie = null;
                        break;
                };
                return new OkObjectResult(movie);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message).ToProblemDetails();
            }
        }

    }
}
