namespace DrVideoLibrary.Api
{
    internal class WatchingEndpoints
    {
        readonly IGetWatchingNowController WatchingNowController;
        readonly IRegisterWatchingNowController RegisterController;
        readonly ILogger Log;

        public WatchingEndpoints(IGetWatchingNowController watchingNowController,
            IRegisterWatchingNowController registerController,
            ILogger log)
        {
            WatchingNowController = watchingNowController;
            RegisterController = registerController;
            Log = log;
        }

        [FunctionName("GetWatching")]
        public async Task<IActionResult> GetWatching(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "watching")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Get actual movie watching");
            Log.LogInformation("Get actual movie watching injected logger");

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


        [FunctionName("RegisterWatchingNow")]
        public async Task<IActionResult> RegisterWatchingNow(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "watching")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Register what movie is watching right now");

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
