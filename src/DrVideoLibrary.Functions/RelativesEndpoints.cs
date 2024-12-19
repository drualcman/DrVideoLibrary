namespace DrVideoLibrary.Functions
{
    internal class RelativesEndpoints
    {
        readonly IGetRelativesController Controller;

        public RelativesEndpoints(IGetRelativesController controller)
        {
            Controller = controller;
        }

        [Function("GetRelatives")]
        public async Task<IActionResult> GetRelatives(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "relatives")] HttpRequest req)
        {
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
