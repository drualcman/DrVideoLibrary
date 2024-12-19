using DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.GetWatchList;

namespace DrVideoLibrary.Api
{
    internal class ListsEndpoints
    {
        readonly IGetAllController GetAllController;
        readonly IGetWatchListController WatchListController;
        public ListsEndpoints(IGetAllController getAllController, IGetWatchListController watchListController)
        {
            GetAllController = getAllController;
            WatchListController = watchListController;
        }

        [FunctionName("GetMovies")]
        public async Task<IActionResult> GetMovies(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "lists/all")] HttpRequest req)
        {
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "lists/watched")] HttpRequest req)
        {
            try
            {
                IEnumerable<WatchedCard> watcheds = await WatchListController.GetWatchList();
                return new OkObjectResult(watcheds);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message).ToProblemDetails();
            }
        }
    }
}
