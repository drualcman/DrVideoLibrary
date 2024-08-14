using System.Diagnostics;

namespace DrVideoLibrary.Razor.Cache.Services;
internal class MoviesCacheService(MoviesContext CacheContext, ApiClient Client, IJSRuntime JsRuntime)
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
        if (cached is null || !cached.Any())
        {
            Console.WriteLine($"GetList not cached");
            movies = await Client.GetMovies();
        }
        else
            movies = cached.Select(MovieCardModel.ToListCard);
        Console.WriteLine($"GetList movies count = {movies.Count()}");
        UpdateCache();
        return movies.OrderBy(m => m.Title).ThenBy(m => m.Year);
    }

    public async Task<IEnumerable<RelativeMovie>> GetRelativesAsync(RelativesDto data)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        MoviesRelations movies = await GetRelations();
        stopwatch.Stop();
        Console.WriteLine($"GetRelativesAsync 1 elapset {stopwatch.Elapsed}");
        stopwatch.Reset();
        List<RelativeMovie> relativeMovies = new List<RelativeMovie>();
        stopwatch.Start();
        switch (data.RelativeOf)
        {
            case RelativeType.ACTOR:
                relativeMovies = await GetMoviesByRelation(movies.Actors, data);
                break;
            case RelativeType.DIRECTOR:
                relativeMovies = await GetMoviesByRelation(movies.Directors, data);
                break;
            case RelativeType.CATEGORY:
                relativeMovies = await GetMoviesByRelation(movies.Categories, data);
                break;
        }
        stopwatch.Stop();
        Console.WriteLine($"GetRelativesAsync 2 elapset {stopwatch.Elapsed}");

        return relativeMovies;
    }

    private async Task<List<RelativeMovie>> GetMoviesByRelation(MovieCounter[] relations, RelativesDto data)
    {
        ConcurrentDictionary<string, RelativeMovie> moviesDict = new();
        List<Task> moviesTask = new();

        foreach (string name in data.Data)
        {
            moviesTask.Add(Task.Run(async () =>
            {
                MovieCounter relation = relations.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                if (relation != null)
                {
                    List<Task> relationTasks = relation.Movies.Select(movie => Task.Run(() =>
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
                    })).ToList();

                    await Task.WhenAll(relationTasks);
                }
            }));
        }

        await Task.WhenAll(moviesTask);
        return moviesDict.Values.ToList();
    }

    public async ValueTask<MoviesRelations> GetRelations()
    {
        MoviesRelations relations = new MoviesRelations();
        IEnumerable<ListCard> cards = await GetList();
        relations.Categories = cards
            .SelectMany(c => c.Categories, (movie, category) => new { movie, category })
            .GroupBy(c => c.category)
            .Select(g => new MovieCounter
            {
                Name = g.Key,
                Movies = g.Select(m => new MovieBasic(m.movie.Id, m.movie.Cover, m.movie.Title)).ToArray()
            })
            .ToArray();
        List<ActorRelationModel> actors = await CacheContext.Actors.SelectAsync();
        if (actors is not null && actors.Any())
            relations.Actors = actors.Select(a => new MovieCounter
            {
                Name = a.Name,
                Movies = a.Movies.Select(m => new MovieBasic(m.MovieId, m.Cover, m.Title)).ToArray()
            }).ToArray();
        List<DirectorRelationModel> directos = await CacheContext.Directors.SelectAsync();
        if (directos is not null && directos.Any())
            relations.Directors = directos.Select(a => new MovieCounter
            {
                Name = a.Name,
                Movies = a.Movies.Select(m => new MovieBasic(m.MovieId, m.Cover, m.Title)).ToArray()
            }).ToArray();

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
