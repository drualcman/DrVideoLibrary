using System.Diagnostics;
using System.IO;

namespace DrVideoLibrary.Razor.Cache.Services;
internal class MoviesCacheService(MoviesContext CacheContext, ApiClient Client, IJSRuntime JsRuntime)
{
    MoviesRelations RelativeMovies;
    public async Task ResetCache()
    {
        await CacheContext.DropDatabaseAsync();
        await CacheContext.Init();
    }

    public async ValueTask<IEnumerable<ListCard>> GetList()
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        IEnumerable<ListCard> movies = [];
        List<MovieCardModel> cached = await CacheContext.Movies.SelectAsync();
        stopwatch.Stop();
        Console.WriteLine($"GetList 1 elapset {stopwatch.Elapsed}");
        stopwatch.Reset();
        stopwatch.Start();
        if (cached is null || !cached.Any())
        {
            Console.WriteLine($"GetList not cached");
            movies = await Client.GetMovies();
        }
        else
            movies = cached.Select(MovieCardModel.ToListCard);
        stopwatch.Stop();
        Console.WriteLine($"GetList 2 elapset {stopwatch.Elapsed}");
        stopwatch.Reset();
        Console.WriteLine($"GetList movies count = {movies.Count()}");
        stopwatch.Start();
        UpdateCache();
        stopwatch.Stop();
        Console.WriteLine($"GetList 3 elapset {stopwatch.Elapsed}");
        return movies.OrderBy(m => m.Title).ThenBy(m => m.Year);
    }

    public async Task ProcessRelatives()
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        if (RelativeMovies is null)
        {
            RelativeMovies = await GetRelations();
        }
        stopwatch.Stop();
        Console.WriteLine($"ProcessRelatives 1 elapset {stopwatch.Elapsed}");
    }

    public async Task<IEnumerable<RelativeMovie>> GetRelativesAsync(RelativesDto data)
    {
        Stopwatch stopwatch = new();
        List<RelativeMovie> relativeMovies = new List<RelativeMovie>();
        stopwatch.Start();
        switch (data.RelativeOf)
        {
            case RelativeType.ACTOR:
                relativeMovies = await GetMoviesByRelation(RelativeMovies.Actors, data);
                break;
            case RelativeType.DIRECTOR:
                relativeMovies = await GetMoviesByRelation(RelativeMovies.Directors, data);
                break;
            case RelativeType.CATEGORY:
                relativeMovies = await GetMoviesByRelation(RelativeMovies.Categories, data);
                break;
        }
        stopwatch.Stop();
        Console.WriteLine($"GetRelativesAsync elapset {stopwatch.Elapsed}");

        return relativeMovies;
    }

    private async Task<List<RelativeMovie>> GetMoviesByRelation(MovieCounter[] relations, RelativesDto data)
    {
        ConcurrentDictionary<string, RelativeMovie> moviesDict = new();
        List<Task> tasks = new();

        foreach (string name in data.Data)
        {
            tasks.Add(Task.Run(() =>
            {
                MovieCounter relation = relations.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                if (relation != null)
                {
                    foreach (var movie in relation.Movies)
                    {
                        moviesDict.TryAdd(movie.Id, new RelativeMovie
                        {
                            Id = movie.Id,
                            Cover = movie.Cover,
                            Title = movie.Title,
                            Data = new RelativesDto
                            {
                                RelativeOf = data.RelativeOf,
                                Data = data.Data
                            }
                        });
                    }
                }
            }));
        }

        await Task.WhenAll(tasks);
        return moviesDict.Values.ToList();
    }

    private async ValueTask<MoviesRelations> GetRelations()
    {
        List<Task> task = new();

        Stopwatch stopwatch = new();
        stopwatch.Start();

        MoviesRelations relations = new();
        IEnumerable<ListCard> cards = await GetList();

        stopwatch.Stop();
        Console.WriteLine($"GetRelations 1 elapset {stopwatch.Elapsed}");

        stopwatch.Reset();
        stopwatch.Start();

        task.Add(Task.Run(() =>
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            relations.Categories = cards
            .SelectMany(c => c.Categories, (movie, category) => new { movie, category })
            .GroupBy(c => c.category)
            .Select(g => new MovieCounter
            {
                Name = g.Key,
                Movies = g.Select(m => new MovieBasic(m.movie.Id, m.movie.Cover, m.movie.Title)).ToArray()
            }).ToArray();
            stopwatch.Stop();

            Console.WriteLine($"GetRelations 2 Categories elapset {stopwatch.Elapsed}");
        }));
        task.Add(Task.Run(async () =>
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            List<ActorRelationModel> actors = await CacheContext.Actors.SelectAsync();

            stopwatch.Stop();
            Console.WriteLine($"GetRelations 3 Actors 1 elapset {stopwatch.Elapsed}");
            stopwatch.Reset();
            stopwatch.Start();
            if (actors is not null && actors.Any())
            {
                relations.Actors = actors.Select(a => new MovieCounter
                {
                    Name = a.Name,
                    Movies = a.Movies.Select(m => new MovieBasic(m.MovieId, m.Cover, m.Title)).ToArray()
                }).ToArray();
            }

            stopwatch.Stop();
            Console.WriteLine($"GetRelations 3 Actors 2 elapset {stopwatch.Elapsed}");

        }));
        task.Add(Task.Run(async () =>
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            List<DirectorRelationModel> directors = await CacheContext.Directors.SelectAsync();
            if (directors is not null && directors.Any())
            {
                relations.Directors = directors.Select(d => new MovieCounter
                {
                    Name = d.Name,
                    Movies = d.Movies.Select(m => new MovieBasic(m.MovieId, m.Cover, m.Title)).ToArray()
                }).ToArray();
            }
            stopwatch.Stop();

            Console.WriteLine($"GetRelations 4 Directors elapset {stopwatch.Elapsed}");

        }));
        await Task.WhenAll(task);

        Console.WriteLine($"GetRelations 2 elapset {stopwatch.Elapsed}");
        return relations;
    }

    private void UpdateCache()
    {
        Console.WriteLine($"UpdateCache");
        Task.Run(async () =>
        {
            bool needUpdate = await GetShouldUpdate();
            if (needUpdate)
            {
                Console.WriteLine($"UpdateCache needUpdate 1");
                IEnumerable<ListCard> toUpdate = await Client.GetMovies();
                Console.WriteLine($"UpdateCache toUpdate count = {toUpdate.Count()}");
                await CacheContext.Movies.CleanAsync();
                await CacheContext.Movies.AddAsync(toUpdate.Select(MovieCardModel.FromListCard).ToList());
                await JsRuntime.InvokeAsync<DateTime?>("localStorage.setItem", "last-update", DateTime.UtcNow);
            }
        });
        Task.Run(async () =>
        {
            DateTime actual = DateTime.Today;
            DateTime last = actual.AddYears(-1);
            try
            {
                DateTime? lastUpdate = await JsRuntime.InvokeAsync<DateTime?>("localStorage.getItem", "last-relations-update");
                if (lastUpdate.HasValue)
                    last = lastUpdate.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            TimeSpan difference = actual.Subtract(last);
            int daysPassed = (int)difference.TotalDays;
            bool needUpdate = daysPassed > 30;
            if (needUpdate)
            {
                Console.WriteLine($"UpdateCache needUpdate 2");
                IEnumerable<MovieRelationDto> toUpdate = await Client.GetRelativesAsync();

                Console.WriteLine($"UpdateCache needUpdate count {toUpdate.Count()}");
                MoviesRelations relations = CreateMoviesRelations(toUpdate);
                Console.WriteLine($"UpdateCache relations.Actors count {relations.Actors.Count()}");
                Console.WriteLine($"UpdateCache relations.Directors count {relations.Directors.Count()}");
                await CacheContext.Actors.CleanAsync();
                await CacheContext.Actors.AddAsync(relations.Actors.Select(d => new ActorRelationModel
                {
                    Count = d.Count,
                    Movies = d.Movies.Select(m => new MovieBasicModel
                    {
                        Cover = m.Cover,
                        Title = m.Title,
                        MovieId = m.Id
                    }).ToList(),
                    Name = d.Name
                }).ToList());
                await CacheContext.Directors.CleanAsync();
                await CacheContext.Directors.AddAsync(relations.Directors.Select(d => new DirectorRelationModel
                {
                    Count = d.Count,
                    Movies = d.Movies.Select(m => new MovieBasicModel
                    {
                        Cover = m.Cover,
                        Title = m.Title,
                        MovieId = m.Id
                    }).ToList(),
                    Name = d.Name
                }).ToList());
                await JsRuntime.InvokeAsync<DateTime?>("localStorage.setItem", "last-relations-update", DateTime.UtcNow);
            }
        });
    }

    private MoviesRelations CreateMoviesRelations(IEnumerable<MovieRelationDto> moviesDetails)
    {
        Dictionary<string, List<MovieBasic>> actorMovies = new Dictionary<string, List<MovieBasic>>();
        Dictionary<string, List<MovieBasic>> directorMovies = new Dictionary<string, List<MovieBasic>>();

        foreach (MovieRelationDto movie in moviesDetails)
        {
            MovieBasic movieBasic = new MovieBasic(movie.Id, movie.Cover, movie.Title);

            foreach (string actor in movie.Actors)
            {
                if (!actorMovies.ContainsKey(actor))
                    actorMovies[actor] = new List<MovieBasic>();
                actorMovies[actor].Add(movieBasic);
            }

            foreach (string director in movie.Directors)
            {
                if (!directorMovies.ContainsKey(director))
                    directorMovies[director] = new List<MovieBasic>();
                directorMovies[director].Add(movieBasic);
            }
        }

        MovieCounter[] actors = actorMovies.Select(am => new MovieCounter { Name = am.Key, Movies = am.Value.ToArray() }).ToArray();
        MovieCounter[] directors = directorMovies.Select(dm => new MovieCounter { Name = dm.Key, Movies = dm.Value.ToArray() }).ToArray();

        return new MoviesRelations
        {
            Actors = actors,
            Directors = directors
        };
    }

    private async ValueTask<bool> GetShouldUpdate()
    {
        bool result = await GetShouldDayUpdate();
        try
        {
            if (!result)
            {
                string data = await JsRuntime.InvokeAsync<string>("localStorage.getItem", "CATALOG");
                result = Convert.ToBoolean(data);
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
