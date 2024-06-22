using System.IO;

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
        string queryString = "SELECT c.id, c.title, c.originaltitle, c.cover, c.year, c.description, c.rate, c.duration, c.categories, c.directors, c.actors FROM c WHERE c.register = 'movies'";
        return await GetMoviesCollection(new QueryDefinition(queryString));
    }

    public async Task<IEnumerable<MovieModel>> GetAllByActors(string[] actors)
    {
        List<string> conditions = new List<string>();

        if (actors != null && actors.Length > 0)
        {
            string actorCondition = string.Join(" OR ", actors.Select((actor, index) => $"ARRAY_CONTAINS(c.actors, @value{index})"));
            conditions.Add($"({actorCondition})");
        }
        actors = actors.Select(actor => actor.Replace("'", "''")).ToArray();
        return await GetAllBy(conditions, actors);
    }

    public async Task<IEnumerable<MovieModel>> GetAllByDirectors(string[] directors)
    {
        List<string> conditions = new List<string>();

        if (directors != null && directors.Length > 0)
        {
            string directorCondition = string.Join(" OR ", directors.Select((director, index) => $"ARRAY_CONTAINS(c.directors, @value{index})"));
            conditions.Add($"({directorCondition})");
        }
        directors = directors.Select(director => director.Replace("'", "''")).ToArray();
        return await GetAllBy(conditions, directors);
    }

    public async Task<IEnumerable<MovieModel>> GetAllByCategories(string[] categories)
    {
        List<string> conditions = new List<string>();
        if (categories != null && categories.Length > 0)
        {
            string categoryCondition = string.Join(" OR ", categories.Select((category, index) => $"ARRAY_CONTAINS(c.categories, @value{index})"));
            conditions.Add($"({categoryCondition})");
        }
        categories = categories.Select(category => category.Replace("'", "''")).ToArray();
        return await GetAllBy(conditions, categories);
    }

    private async Task<IEnumerable<MovieModel>> GetAllBy(IEnumerable<string> conditions, string[] values)
    {
        string conditionsString = string.Join(" AND ", conditions);

        string queryString = $"SELECT c.id, c.title, c.originaltitle, c.cover, c.year, c.description, c.rate, c.duration, c.categories, c.directors, c.actors FROM c WHERE c.register = 'movies'";

        if (!string.IsNullOrEmpty(conditionsString))
        {
            queryString += $" AND ({conditionsString})";
        }

        QueryDefinition queryDefinition = new QueryDefinition(queryString);
        for (int i = 0; i < values.Length; i++)
        {
            queryDefinition.WithParameter($"@value{i}", values[i]);
        }

        return await GetMoviesCollection(queryDefinition);
    }


    private async Task<IEnumerable<MovieModel>> GetMoviesCollection(QueryDefinition queryDefinition)
    {
        FeedIterator<MovieModel> query = GetContainer().GetItemQueryIterator<MovieModel>(queryDefinition);
        ConcurrentBag<MovieModel> results = new ConcurrentBag<MovieModel>();
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

    public async Task<RegisterView> GetWatchingNow()
    {
        string queryString = $"SELECT top 1 c.id, c.movieid, c.start FROM c WHERE c.register = 'views' ORDER BY c.start desc";
        FeedIterator<RegisterView> query = GetContainer().GetItemQueryIterator<RegisterView>(new QueryDefinition(queryString));
        RegisterView result = null;
        if (query.HasMoreResults)
        {
            FeedResponse<RegisterView> response = await query.ReadNextAsync();
            result = response.FirstOrDefault();
        }
        return result;
    }

    public async Task<IEnumerable<RegisterView>> GetTotalViews(string movieId)
    {
        string queryString = $"SELECT c.id, c.start FROM c WHERE c.register = 'views' AND c.movieid = '{movieId}'";
        FeedIterator<RegisterView> query = GetContainer().GetItemQueryIterator<RegisterView>(new QueryDefinition(queryString));
        ConcurrentBag<RegisterView> results = new ConcurrentBag<RegisterView>();
        List<Task> tasks = new List<Task>();
        while (query.HasMoreResults)
        {
            FeedResponse<RegisterView> response = await query.ReadNextAsync();
            tasks.AddRange(response.Select(movie => Task.Run(() => results.Add(movie))));
        }
        await Task.WhenAll(tasks);
        return results;
    }

    public async Task<MovieModel> GetMovieById(string id)
    {
        string queryString = $"SELECT c.id, c.title, c.originaltitle, c.cover, c.year, c.description, c.rate, c.duration, c.categories, c. directors, c.actors FROM c WHERE c.register = 'movies' AND c.id = '{id}'";
        FeedIterator<MovieModel> query = GetContainer().GetItemQueryIterator<MovieModel>(new QueryDefinition(queryString));
        MovieModel result = null;
        if (query.HasMoreResults)
        {
            FeedResponse<MovieModel> response = await query.ReadNextAsync();
            result = response.FirstOrDefault();
        }
        return result;
    }
}
