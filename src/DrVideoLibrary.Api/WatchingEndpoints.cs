using DrVideoLibrary.Backend.ApplicationBusinessRules.Interfaces.UseCases.GetWhatchingNow;

namespace DrVideoLibrary.Api
{
    internal class WatchingEndpoints
    {
        readonly IGetWhatchingNowController WatchingNowController;
        readonly IRegisterWatchingNowController RegisterController;

        public WatchingEndpoints(IGetWhatchingNowController watchingNowController, IRegisterWatchingNowController registerController)
        {
            WatchingNowController = watchingNowController;
            RegisterController = registerController;
        }

        [FunctionName("GetWhatching")]
        public async Task<IActionResult> GetWhatching(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "watching")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Get actual movie watching");

            try
            {
                WatchingNow result = await WatchingNowController.GetWhatchingNow();
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
                string id = await HttpRequestHelper.GetRequestedModel<string>(req);
                await RegisterController.RegisterWatchingNow(id);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message).ToProblemDetails();
            }
        }
    }
}
