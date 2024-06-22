namespace DrVideoLibrary.Api
{
    internal class NotificationsEndpoints
    {
        readonly INotificationSubscribeController SubscribeController;
        public NotificationsEndpoints(INotificationSubscribeController subuscribe)
        {
            SubscribeController = subuscribe;
        }

        [FunctionName("Subscribe")]
        public async Task<IActionResult> Subscribe(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "notification")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Subscribe for notification");

            try
            {
                NotificationSubscription result = await HttpRequestHelper.GetRequestedModel<NotificationSubscription>(req);
                await SubscribeController.NotificationSubscribe(result);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message).ToProblemDetails();
            }
        }
    }
}
