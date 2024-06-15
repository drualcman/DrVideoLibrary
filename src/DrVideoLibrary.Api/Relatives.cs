namespace DrVideoLibrary.Api
{
    public class Relatives
    {
        readonly IGetRelativesController Controller;

        public Relatives(IGetRelativesController controller)
        {
            Controller = controller;
        }

        [FunctionName("relatives")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "relatives/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            log.LogInformation("Get all relatives movies");

            try
            {
                var result = await Controller.GetRelatives(id);
                return new OkObjectResult(result);
            }
            catch (System.Exception ex)
            {
                return new BadRequestObjectResult(ex.Message).ToProblemDetails();
            }
        }
    }
}
