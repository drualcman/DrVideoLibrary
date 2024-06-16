namespace DrVideoLibrary.Api
{
    internal class ListsEndpoints
    {
        public ListsEndpoints()
        {
        }

        [FunctionName("GetMovies")]
        public async Task<IActionResult> GetMovies(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "lists/all")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Get movie list");

            try
            {
                await Task.Delay(1);
                List<ListCard> movies = new List<ListCard>()
                {
                    new ListCard(Guid.NewGuid().ToString(), "Un nacimiento de un ser muy especial en la tierra media de nuestro señor", "", 1976, new string[] {"miedo", "terror" }),
                    new ListCard(Guid.NewGuid().ToString(), "El traviero", "", 1979, new string[] {"comedia", "drama" }),
                    new ListCard(Guid.NewGuid().ToString(), "El colegio, que miedo", "", 1982, new string[] {"ciencia ficcion", "comedia", "accion", "drama" })
                };

                for (int i = 0; i < 1000; i++)
                {
                    movies.Add(new ListCard(Guid.NewGuid().ToString(), "El traviero", "", 1979, new string[] { "comedia", "drama" }));
                }
                return new OkObjectResult(movies);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message).ToProblemDetails();
            }
        } 

        [FunctionName("GetWatchedList")]
        public async Task<IActionResult> GetWatchedList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "lists/watched")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Get watched list");

            try
            {
                await Task.Delay(1);
                List<WatchedCard> watcheds = new List<WatchedCard>()
                {
                    new WatchedCard(Guid.NewGuid().ToString(), "Start Wars Episodio I", "", 215, 80),
                    new WatchedCard(Guid.NewGuid().ToString(), "Start Wars Episodio II", "", 215, 75),
                    new WatchedCard(Guid.NewGuid().ToString(), "Start Wars Episodio III", "", 215, 65),
                    new WatchedCard(Guid.NewGuid().ToString(), "Start Wars Episodio IV", "", 215, 100),
                    new WatchedCard(Guid.NewGuid().ToString(), "Start Wars Episodio V", "", 215, 95),
                    new WatchedCard(Guid.NewGuid().ToString(), "Start Wars Episodio VI", "", 215, 100),
                    new WatchedCard(Guid.NewGuid().ToString(), "Start Wars Episodio VII", "", 215, 50),
                    new WatchedCard(Guid.NewGuid().ToString(), "Start Wars Episodio VIII", "", 215, 90),
                    new WatchedCard(Guid.NewGuid().ToString(), "Start Wars Episodio IX", "", 215, 85),
                };
                for (int i = 0; i < 100; i++)
                {
                    watcheds.Add(new WatchedCard(Guid.NewGuid().ToString(), "Popelle el marino", "", 215, 80));
                }
                return new OkObjectResult(watcheds);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message).ToProblemDetails();
            }
        }
    }
}
