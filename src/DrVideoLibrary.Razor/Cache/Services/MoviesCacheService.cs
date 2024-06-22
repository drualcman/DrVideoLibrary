namespace DrVideoLibrary.Razor.Cache.Services;
public class MoviesCacheService(MoviesContext CacheContext, ApiClient Client)
{
    public async ValueTask<List<ListCard>> GetList()
    {
        List<ListCard> movies = [];
        List<MovieCardModel> cached = await CacheContext.Movies.SelectAsync();
        if(cached == null || !cached.Any())
        {
            movies = new(await Client.GetMovies());
            _ =CacheContext.Movies.AddAsync(movies.Select(MovieCardModel.FromListCard).ToList());
        }
        else
        {
            movies = cached.Select(MovieCardModel.ToListCard).ToList();
        }
        return movies;
    }

}
