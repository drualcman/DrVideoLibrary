using DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.GetMovieById;

namespace DrVideoLibrary.Api
{
    internal class MovieEndpoints
    {
        readonly IAddMovieController AddMovieController;
        readonly IGetMovieByIdController GetMovieByIdController;

        public MovieEndpoints(IAddMovieController addMovieController, IGetMovieByIdController getMovieByIdController)
        {
            AddMovieController = addMovieController;
            GetMovieByIdController = getMovieByIdController;
        }

        [FunctionName("GetMovieById")]
        public async Task<IActionResult> GetMovieById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "movie/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            log.LogInformation("Get movie details");

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

        [FunctionName("AddMovie")]
        public async Task<IActionResult> AddMovie(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "movie")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Add new movie");

            try
            {
                Movie data = await HttpRequestHelper.GetRequestedModel<Movie>(req);
                await AddMovieController.AddMovie(data, log);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message).ToProblemDetails();
            }
        }


    }
}
