namespace DrVideoLibrary.Api
{
    internal class MovieEndpoints
    {
        readonly IAddMovieController AddMovieController;

        public MovieEndpoints(IAddMovieController addMovieController)
        {
            AddMovieController = addMovieController;
        }

        [FunctionName("GetMovieById")]
        public async Task<IActionResult> GetMovieById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "movie/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            log.LogInformation("Get movie details");

            try
            {
                await Task.Delay(1);
                var result = new Movie
                {
                    Id = id,
                    Title = $"[{id}] The Matrix",
                    Cover = "https://books.community-mall.com/images/file070825676587736583778817187786848566707281006846872787106475657384.jpg",
                    Year = 1999,
                    Description = "In a dystopian future, humanity is unknowingly trapped inside a simulated reality, the Matrix, created by intelligent machines to distract humans while using their bodies as an energy source.",
                    Rate = 9,
                    Duration = 136,
                    TotalViews = 69,
                    Categories = new List<string> { "Action", "Drama", "Sci-Fi" },
                    Directors = new List<string> { "Lana Wachowski", "Lilly Wachowski" },
                    Actors = new List<string> { "Keanu Reeves", "Laurence Fishburne", "Carrie-Anne Moss" }
                };
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
