namespace DrVideoLibrary.Functions
{
    internal class WatchingEndpoints
    {
        readonly IGetWatchingNowController WatchingNowController;
        readonly IRegisterWatchingNowController RegisterController;

        public WatchingEndpoints(IGetWatchingNowController watchingNowController,
            IRegisterWatchingNowController registerController)
        {
            WatchingNowController = watchingNowController;
            RegisterController = registerController;
        }

        [Function("GetWatching")]
        public async Task<IActionResult> GetWatching(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "watching")] HttpRequest req)
        {
            try
            {
                WatchingNow result = await WatchingNowController.GetWatchingNow();
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message).ToProblemDetails();
            }
        }


        [Function("RegisterWatchingNow")]
        public async Task<IActionResult> RegisterWatchingNow(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "watching")] HttpRequest req)
        {
            try
            {
                WatchingNowDto data = await HttpRequestHelper.GetRequestedModel<WatchingNowDto>(req);
                await RegisterController.RegisterWatchingNow(data);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message).ToProblemDetails();
            }
        }
    }
}
