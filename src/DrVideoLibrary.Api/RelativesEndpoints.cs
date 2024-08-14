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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "relatives")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Get all relatives movies");

            try
            {
                IEnumerable<MovieRelationDto> result = await Controller.GetRelatives();
                return new OkObjectResult(result.OrderBy(m => m.Title));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message).ToProblemDetails();
            }
        }
    }
}
