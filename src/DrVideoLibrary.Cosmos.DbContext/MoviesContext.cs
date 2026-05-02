namespace DrVideoLibrary.Cosmos.DbContext;

internal class MoviesContext : IMoviesContext
{
    const string MoviesContainer = "Movies";
    readonly PartitionKey Movies = new PartitionKey("movies");
    readonly PartitionKey Views = new PartitionKey("views");

    readonly CosmosClient CosmosClient;
    readonly CosmosClient BackupClient;
    public MoviesContext(IOptions<ConnectionStringsOptions> connectionStrings)
    {
        CosmosClient = new CosmosClient(connectionStrings.Value.Database,
            new CosmosClientOptions
            {
                AllowBulkExecution = true
            });

        BackupClient = new CosmosClient(connectionStrings.Value.Backup,
            new CosmosClientOptions
            {
                AllowBulkExecution = true
            });
    }

    Container GetContainer() => CosmosClient.GetContainer("SOG_Personal_Projects", MoviesContainer);

    public async Task AddMovie(MovieModel data)
    {
        await GetContainer().UpsertItemAsync(ObjectConverter.ConvertToLowercaseObject(data, "movies"), Movies);
    }

    public async Task<IEnumerable<MovieModel>> GetMoviesAll()
    {
        string queryString = "SELECT c.id, c.title, c.originaltitle, c.cover, c.year, c.description, c.rate, c.duration, c.categories, c.directors, c.actors FROM c WHERE c.register = 'movies'";
        return await GetCollection<MovieModel>(new QueryDefinition(queryString));
    }

    public async Task RegisterWatchingNow(RegisterView data)
    {
        await GetContainer().UpsertItemAsync(ObjectConverter.ConvertToLowercaseObject(data, "views"), Views);
    }

    public async Task<RegisterView> GetWatchingNow()
    {
        string queryString = $"SELECT top 1 c.id, c.movieid, c.start FROM c WHERE c.register = 'views' ORDER BY c.start desc";
        return await GetSingle<RegisterView>(new QueryDefinition(queryString));
    }

    private async Task<IEnumerable<TModel>> GetCollection<TModel>(QueryDefinition queryDefinition) where TModel : class
    {
        try
        {
            FeedIterator<TModel> query = GetContainer().GetItemQueryIterator<TModel>(queryDefinition);
            ConcurrentBag<TModel> results = new ConcurrentBag<TModel>();
            List<Task> tasks = new List<Task>();
            while (query.HasMoreResults)
            {
                FeedResponse<TModel> response = await query.ReadNextAsync();
                tasks.AddRange(response.Select(item => Task.Run(() => results.Add(item))));
            }
            await Task.WhenAll(tasks);
            return results;
        }
        catch (Exception ex)
        {
            string k = ex.ToString();
            throw;
        }
    }

    private async Task<TModel> GetSingle<TModel>(QueryDefinition queryDefinition) where TModel : class
    {
        FeedIterator<TModel> query = GetContainer().GetItemQueryIterator<TModel>(queryDefinition);
        TModel result = default;
        if (query.HasMoreResults)
        {
            FeedResponse<TModel> response = await query.ReadNextAsync();
            result = response.FirstOrDefault();
        }
        return result;
    }

    public async Task<IEnumerable<RegisterView>> GetTotalViews(string movieId)
    {
        string queryString = $"SELECT c.id, c.start FROM c WHERE c.register = 'views' AND c.movieid = '{movieId}'";
        return await GetCollection<RegisterView>(new QueryDefinition(queryString));
    }

    public async Task<MovieModel> GetMovieById(string id)
    {
        string queryString = $"SELECT c.id, c.title, c.originaltitle, c.cover, c.year, c.description, c.rate, c.duration, c.categories, c. directors, c.actors FROM c WHERE c.register = 'movies' AND c.id = '{id}'";
        return await GetSingle<MovieModel>(new QueryDefinition(queryString));
    }

    public async Task<IEnumerable<WatchedModel>> GetWatchedMoviesAll()
    {
        string queryString = "SELECT c.id, c.title, c.cover, c.duration, c.rate FROM c WHERE c.register = 'movies'";
        return await GetCollection<WatchedModel>(new QueryDefinition(queryString));
    }

    public async Task<IEnumerable<WatchedCountModel>> GetWatchedViewsAll()
    {
        string queryString = "SELECT v.movieid, COUNT(1) AS times FROM c v WHERE v.register = 'views' GROUP BY v.movieid";
        return await GetCollection<WatchedCountModel>(new QueryDefinition(queryString));
    }

    public async Task ExecuteBackup()
    {
        Container sourceContainer = CosmosClient.GetContainer("BDStartups", MoviesContainer);
        Container destinationContainer = BackupClient.GetContainer("SOG_Personal_Projects", MoviesContainer);

        QueryDefinition queryDefinition = new QueryDefinition("SELECT * FROM c");

        FeedIterator<dynamic> iterator = sourceContainer.GetItemQueryIterator<dynamic>(queryDefinition);

        while (iterator.HasMoreResults)
        {
            FeedResponse<dynamic> response = await iterator.ReadNextAsync();

            List<Task> tasks = new List<Task>();

            IEnumerator<dynamic> enumerator = response.GetEnumerator();

            while (enumerator.MoveNext())
            {
                dynamic item = enumerator.Current;

                tasks.Add(destinationContainer.UpsertItemAsync(item));
            }

            await Task.WhenAll(tasks);
        }
    }
}
