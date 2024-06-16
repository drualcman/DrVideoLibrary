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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "relatives/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            log.LogInformation("Get all relatives movies");

            try
            {
                var result = await Controller.GetRelatives(id);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message).ToProblemDetails();
            }
        }

    }
}
