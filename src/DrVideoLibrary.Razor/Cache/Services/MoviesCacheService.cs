﻿namespace DrVideoLibrary.Razor.Cache.Services;
public class MoviesCacheService(MoviesContext CacheContext, ApiClient Client, IJSRuntime JsRuntime)
{
    public async Task ResetCache()
    {
        await CacheContext.DropDatabaseAsync();
        await CacheContext.Init();
    }

    public async ValueTask<IEnumerable<ListCard>> GetList()
    {
        IEnumerable<ListCard> movies = [];
        List<MovieCardModel> cached = await CacheContext.Movies.SelectAsync();
        if (await GetShouldUpdate())
        {
            cached = null;
        }
        if (cached == null || !cached.Any())
        {
            movies = await Client.GetMovies();
            UpdateCache();
        }
        else
        {
            movies = cached.Select(MovieCardModel.ToListCard);
        }
        return movies.OrderBy(m => m.Title).ThenBy(m => m.Year);
    }

    private void UpdateCache()
    {
        Task.Run(async () =>
        {
            IEnumerable<ListCard> movies = await Client.GetMovies();
            await CacheContext.Movies.CleanAsync();
            await CacheContext.Movies.AddAsync(movies.Select(MovieCardModel.FromListCard).ToList());
            await JsRuntime.InvokeAsync<DateTime?>("localStorage.setItem", "last-update", DateTime.UtcNow);
        });
    }

    private async ValueTask<bool> GetShouldUpdate()
    {
        bool result = await GetShouldDayUpdate();
        try
        {
            if (!result)
            {
                string data = await JsRuntime.InvokeAsync<string>("localStorage.getItem", "CATALOG");
                result = Convert.ToBoolean( data );
                await JsRuntime.InvokeAsync<string>("localStorage.setItem", "CATALOG", false);
            }
        }
        catch
        {
            result = await GetShouldDayUpdate();
        }
        return result;
    }

    private async ValueTask<bool> GetShouldDayUpdate()
    {
        bool result;
        try
        {
            DateTime? lastUpdate = await JsRuntime.InvokeAsync<DateTime?>("localStorage.getItem", "last-update");
            if (lastUpdate.HasValue)
            {
                DateOnly last = DateOnly.FromDateTime(lastUpdate.Value);
                DateOnly actual = DateOnly.FromDateTime(DateTime.Today);
                result = last != actual;
            }
            else result = true;
        }
        catch
        {
            result = true;
        }
        return result;

    }

}
