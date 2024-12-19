namespace DrVideoLibrary.Functions
{
    internal class NotificationsEndpoints
    {
        readonly INotificationSubscribeController SubscribeController;
        public NotificationsEndpoints(INotificationSubscribeController subuscribe)
        {
            SubscribeController = subuscribe;
        }

        [Function("Subscribe")]
        public async Task<IActionResult> Subscribe(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "notification")] HttpRequest req)
        {
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
