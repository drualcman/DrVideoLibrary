namespace DrVideoLibrary.Cosmos.DbContext;
internal class MoviesContext : IMoviesContext
{
    const string MoviesContainer = "Movies";
    readonly PartitionKey Movies = new PartitionKey("movies");
    readonly PartitionKey Views = new PartitionKey("views");

    readonly CosmosClient CosmosClient;
    public MoviesContext(IOptions<ConnectionStringsOptions> connectionStrings)
    {
        CosmosClient = new CosmosClient(connectionStrings.Value.Database);
    }

    Container GetContainer() => CosmosClient.GetContainer("BDStartups", MoviesContainer);

    public async Task AddMovie(MovieModel data)
    {
        await GetContainer().UpsertItemAsync(ObjectConverter.ConvertToLowercaseObject(data, "movies"), Movies);
    }

    public async Task<IEnumerable<MovieModel>> GetAll()
    {
        string queryString = "SELECT c.id, c.title, c.originaltitle, c.cover, c.year, c.description, c.rate, c.duration, c.categories, c. directors, c.actors FROM c";
        FeedIterator<MovieModel> query = GetContainer().GetItemQueryIterator<MovieModel>(new QueryDefinition(queryString));
        List<MovieModel> results = new List<MovieModel>();
        List<Task> tasks = new List<Task>();
        while (query.HasMoreResults)
        {
            FeedResponse<MovieModel> response = await query.ReadNextAsync();
            tasks.AddRange(response.Select(movie => Task.Run(() => results.Add(movie))));
        }
        await Task.WhenAll(tasks);
        return results;
    }

    public async Task RegisterWatchingNow(RegisterView data)
    {
        await GetContainer().UpsertItemAsync(ObjectConverter.ConvertToLowercaseObject(data, "views"), Views);
    }
}
