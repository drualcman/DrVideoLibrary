namespace DrVideoLibrary.Api
{
    internal class ListsEndpoints
    {
        readonly IGetAllController GetAllController;
        public ListsEndpoints(IGetAllController getAllController)
        {
            GetAllController = getAllController;
        }

        [FunctionName("GetMovies")]
        public async Task<IActionResult> GetMovies(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "lists/all")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Get movie list");

            try
            {
                IEnumerable<ListCard> movies = await GetAllController.GetAll();
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
