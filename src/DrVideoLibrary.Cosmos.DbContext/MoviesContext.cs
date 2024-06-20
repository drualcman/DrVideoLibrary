using DrVideoLibrary.Entities.Models;
using System.Collections.Concurrent;

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
        return await GetMoviesCollection(queryString);
    } 

    public async Task<IEnumerable<MovieModel>> GetAllByActors(string[] actors)
    {
        List<string> conditions = new List<string>();

        if (actors != null && actors.Length > 0)
        {
            string actorCondition = string.Join(" OR ", actors.Select(actor => $"ARRAY_CONTAINS(c.actors, '{actor}')"));
            conditions.Add($"({actorCondition})");
        }
        return await GetAllBy(conditions);
    }
    public async Task<IEnumerable<MovieModel>> GetAllByDirectors(string[] directors)
    {
        List<string> conditions = new List<string>();

        if (directors != null && directors.Length > 0)
        {
            string directorCondition = string.Join(" OR ", directors.Select(director => $"ARRAY_CONTAINS(c.directors, '{director}')"));
            conditions.Add($"({directorCondition})");
        }
        return await GetAllBy(conditions);
    }
    public async Task<IEnumerable<MovieModel>> GetAllByCategories(string[] categories)
    {
        List<string> conditions = new List<string>();
        if (categories != null && categories.Length > 0)
        {
            string categoryCondition = string.Join(" OR ", categories.Select(category => $"ARRAY_CONTAINS(c.categories, '{category}')"));
            conditions.Add($"({categoryCondition})");
        }
        return await GetAllBy(conditions);
    } 
    
    private async Task<IEnumerable<MovieModel>> GetAllBy(IEnumerable<string> conditions)
    {
        string conditionsString = string.Join(" OR ", conditions);

        string queryString = $"SELECT c.id, c.title, c.originaltitle, c.cover, c.year, c.description, c.rate, c.duration, c.categories, c.directors, c.actors FROM c WHERE c.register = 'movies'";

        if (!string.IsNullOrEmpty(conditionsString))
        {
            queryString += $" AND ({conditionsString})";
        }
        return await GetMoviesCollection(queryString);
    }


    private async Task<IEnumerable<MovieModel>> GetMoviesCollection(string queryString)
    {
        FeedIterator<MovieModel> query = GetContainer().GetItemQueryIterator<MovieModel>(new QueryDefinition(queryString));
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
