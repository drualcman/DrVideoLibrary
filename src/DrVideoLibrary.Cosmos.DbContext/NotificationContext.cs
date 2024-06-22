namespace DrVideoLibrary.Cosmos.DbContext;
internal class NotificationContext : INotificationContext
{
    const string SubcriptionsContainer = "Movies";
    readonly PartitionKey Subscriptions = new PartitionKey("subscriptions");

    readonly CosmosClient CosmosClient;
    public NotificationContext(IOptions<ConnectionStringsOptions> connectionStrings)
    {
        CosmosClient = new CosmosClient(connectionStrings.Value.Database);
    }

    Container GetContainer() => CosmosClient.GetContainer("BDStartups", SubcriptionsContainer);

    public async Task UpsertSubscribe(SubscriptionModel subscription)
    {
        await GetContainer()
            .UpsertItemAsync(ObjectConverter.ConvertToLowercaseObject(subscription, "subscriptions"), Subscriptions);
    }

    public async Task<IEnumerable<SubscriptionModel>> GetSubscriptions()
    {
        string queryString = $"SELECT c.id, c.userid, c.url, c.p256dh, c.auth  FROM c WHERE c.register = 'subscriptions'";
        FeedIterator<SubscriptionModel> query = GetContainer()
            .GetItemQueryIterator<SubscriptionModel>(new QueryDefinition(queryString));
        ConcurrentBag<SubscriptionModel> results = new ConcurrentBag<SubscriptionModel>();
        List<Task> tasks = new List<Task>();
        while (query.HasMoreResults)
        {
            FeedResponse<SubscriptionModel> response = await query.ReadNextAsync();
            tasks.AddRange(response.Select(movie => Task.Run(() => results.Add(movie))));
        }
        await Task.WhenAll(tasks);
        return results;
    }

    public async Task DeleteSubscription(string userId, string auth)
    {
        string queryString = $"SELECT c.id, c.userid, c.url, c.p256dh, c.auth  FROM c WHERE c.register = 'subscriptions' AND c.userid = '{userId}' AND c.auth = '{auth}'";
        FeedIterator<SubscriptionModel> query = GetContainer()
            .GetItemQueryIterator<SubscriptionModel>(new QueryDefinition(queryString));
        SubscriptionModel subscription = null;
        if (query.HasMoreResults)
        {
            FeedResponse<SubscriptionModel> response = await query.ReadNextAsync();
            subscription = response.FirstOrDefault();
        }
        if (subscription != null) 
        {
            await GetContainer()
                .DeleteItemAsync<SubscriptionModel>(subscription.Id, Subscriptions);
        }        
    }
}
