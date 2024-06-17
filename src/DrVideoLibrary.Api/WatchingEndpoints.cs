namespace DrVideoLibrary.Api
{
    internal class WatchingEndpoints
    {
        readonly IGetRelativesController Controller;

        public WatchingEndpoints()
        {
        }


        [FunctionName("GetWhatching")]
        public async Task<IActionResult> GetWhatching(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "watching")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Get actual movie watching");

            try
            {
                await Task.Delay(1);
                string id = Guid.NewGuid().ToString();
                var result = new WatchingNow
                {
                    Movie = new Entities.Models.Movie
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
                    },
                    Start = DateTime.Now.AddMinutes(-75)
                };
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message).ToProblemDetails();
            }
        }


    }
}
