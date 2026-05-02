using DrVideoLibrary.Backend.Repositories.Interfaces;

namespace DrVideoLibrary.Functions
{
    internal class MovieEndpoints
    {
        readonly IAddMovieController AddMovieController;
        readonly IGetMovieByIdController GetMovieByIdController;
        readonly IMoviesContext MoviesContext;

        public MovieEndpoints(IAddMovieController addMovieController, IGetMovieByIdController getMovieByIdController,
            IMoviesContext moviesContext)
        {
            AddMovieController = addMovieController;
            GetMovieByIdController = getMovieByIdController;
            MoviesContext = moviesContext;
        }

        [Function("Migration")]
        public async Task<IActionResult> ExecuteMigration(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "execute-backup")] HttpRequest req)
        {

            try
            {
                await MoviesContext.ExecuteBackup();
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message).ToProblemDetails();
            }
        }

        [Function("GetMovieById")]
        public async Task<IActionResult> GetMovieById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "movie/{id}")] HttpRequest req, string id)
        {

            try
            {
                Movie result = await GetMovieByIdController.GetMovieById(id);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message).ToProblemDetails();
            }
        }

        [Function("AddMovie")]
        public async Task<IActionResult> AddMovie(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "movie")] HttpRequest req)
        {
            try
            {
                Movie data = await HttpRequestHelper.GetRequestedModel<Movie>(req);
                await AddMovieController.AddMovie(data);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message).ToProblemDetails();
            }
        }


    }
}
