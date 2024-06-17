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
        string queryString = $"SELECT top 1 c.id, c.start FROM c WHERE c.register = 'views' ORDER BY c.start desc";
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
        string queryString = $"SELECT c.id, c.start FROM c WHERE c.register = 'views' AND c.id = '{movieId}'";
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
