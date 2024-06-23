using DrVideoLibrary.Entities.Dtos;
using System.Linq;

namespace DrVideoLibrary.Api
{
    internal class RelativesEndpoints
    {
        readonly IGetRelativesController Controller;

        public RelativesEndpoints(IGetRelativesController controller)
        {
            Controller = controller;
        }

        [FunctionName("GetRelatives")]
        public async Task<IActionResult> GetRelatives(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "relatives")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Get all relatives movies");

            try
            {
                RelativesDto data = await HttpRequestHelper.GetRequestedModel<RelativesDto>(req);
                IEnumerable<RelativeMovie> result = await Controller.GetRelatives(data);
                return new OkObjectResult(result.OrderBy(m => m.Title));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message).ToProblemDetails();
            }
        }

    }
}
