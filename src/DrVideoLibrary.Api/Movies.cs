using DrVideoLibrary.Entities.Models;
using System.Collections.Generic;

namespace DrVideoLibrary.Api
{
    public class Movies
    {
        readonly IGetRelativesController Controller;

        public Movies()
        {
        }

        [FunctionName("movies")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "movies/{id}")] HttpRequest req,
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
                    Prologo = "In a dystopian future, humanity is unknowingly trapped inside a simulated reality, the Matrix, created by intelligent machines to distract humans while using their bodies as an energy source.",
                    Rate = 9,
                    Duration = 136,
                    Categories = new List<string> { "Action", "Drama", "Sci-Fi" },
                    Directors = new List<string> { "Lana Wachowski", "Lilly Wachowski" },
                    Actors = new List<string> { "Keanu Reeves", "Laurence Fishburne", "Carrie-Anne Moss" }
                };
                return new OkObjectResult(result);
            }
            catch (System.Exception ex)
            {
                return new BadRequestObjectResult(ex.Message).ToProblemDetails();
            }
        }
    }
}
