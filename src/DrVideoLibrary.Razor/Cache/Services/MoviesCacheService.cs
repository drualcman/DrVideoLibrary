﻿namespace DrVideoLibrary.Razor.Cache.Services;
public class MoviesCacheService(MoviesContext CacheContext, ApiClient Client, IJSRuntime JsRuntime)
{
    public async ValueTask<IEnumerable<ListCard>> GetList()
    {
        IEnumerable<ListCard> movies = [];
        List<MovieCardModel> cached = await CacheContext.Movies.SelectAsync();
        if (await GetShouldUpdate())
            cached = null;
        if (cached == null || !cached.Any())
        {
            movies = new List<ListCard>(await Client.GetMovies());
            await CacheContext.Movies.CleanAsync();            
            _ = CacheContext.Movies.AddAsync(movies.Select(MovieCardModel.FromListCard).ToList());
        }
        else
        {
            movies = cached.Select(MovieCardModel.ToListCard);
        }
        return movies.OrderBy(m=> m.Title).ThenBy(m=> m.Year);
    }

    private async ValueTask<bool> GetShouldUpdate()
    {
        bool result;
        try
        {
            result = await JsRuntime.InvokeAsync<bool>("localStorage.getItem", "CATALOG");
        }
        catch
        {
            result = false;
        }
        return result;
    }

}