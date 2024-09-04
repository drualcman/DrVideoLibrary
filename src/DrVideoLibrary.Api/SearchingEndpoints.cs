namespace DrVideoLibrary.Api
{
    internal class SearchingEndpoints
    {
        readonly ISeachMoviesController SearchMoviesService;
        readonly ISearchMoveDetailController SearchMoveDetailController;
        readonly ISearchActorInfoController SearchActorInfoController;
        public SearchingEndpoints(
            ISeachMoviesController searchMoviesService,
            ISearchMoveDetailController searchMoveDetailController,
            ISearchActorInfoController searchActorInfoController)
        {
            SearchMoviesService = searchMoviesService;
            SearchMoveDetailController = searchMoveDetailController;
            SearchActorInfoController = searchActorInfoController;
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
                IEnumerable<SearchMovieResult> data = await SearchMoviesService.SearchMovies(text, lang);
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
                Movie movie = await SearchMoveDetailController.SearchMoveDetail(id, lang);
                return new OkObjectResult(movie);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message).ToProblemDetails();
            }
        } 

        [FunctionName("GetActor")]
        public async Task<IActionResult> GetActor(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "search/actor")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Actor details from name");

            try
            {
                string text = req.Query["s"];
                SearchPersonResult actor = await SearchActorInfoController.SearchActor(text);
                return new OkObjectResult(actor);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message).ToProblemDetails();
            }
        }

    }
}
