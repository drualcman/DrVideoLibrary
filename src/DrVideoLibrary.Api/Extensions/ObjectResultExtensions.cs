namespace DrVideoLibrary.Api.Extensions;
internal static class ObjectResultExtensions
{
    public static IActionResult ToProblemDetails(this ObjectResult result)
    {
        ProblemDetails details = new ProblemDetails();
        details.Status = result.StatusCode;
        details.Title = result.Value.ToString();
        return new ObjectResult(details);
    }
}
