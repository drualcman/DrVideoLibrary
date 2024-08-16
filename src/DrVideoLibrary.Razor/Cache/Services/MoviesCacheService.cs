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
        IEnumerable<ListCard> movies = [];
        List<MovieCardModel> cached = await CacheContext.Movies.SelectAsync();
        if (cached is null || !cached.Any())
        {
            Console.WriteLine($"GetList not cached");
            movies = await Client.GetMovies();
        }
        else
            movies = cached.Select(MovieCardModel.ToListCard);
        UpdateCache();
        return movies.OrderBy(m => m.Title).ThenBy(m => m.Year);
    }

    public async Task ProcessRelatives()
    {
        if (RelativeMovies is null)
        {
            RelativeMovies = await GetRelations();
        }
    }

    public async Task<IEnumerable<RelativeMovie>> GetRelativesAsync(RelativesDto data)
    {
        List<RelativeMovie> relativeMovies = new List<RelativeMovie>();
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

        MoviesRelations relations = new();
        IEnumerable<ListCard> cards = await GetList();

        task.Add(Task.Run(() =>
        {
            relations.Categories = cards
            .SelectMany(c => c.Categories, (movie, category) => new { movie, category })
            .GroupBy(c => c.category)
            .Select(g => new MovieCounter
            {
                Name = g.Key,
                Movies = g.Select(m => new MovieBasic(m.movie.Id, m.movie.Cover, m.movie.Title)).ToArray()
            }).ToArray();
        }));
        task.Add(Task.Run(async () =>
        {
            List<ActorRelationModel> actors = await CacheContext.Actors.SelectAsync();

            if (actors is not null && actors.Any())
            {
                relations.Actors = actors.Select(a => new MovieCounter
                {
                    Name = a.Name,
                    Movies = a.Movies.Select(m => new MovieBasic(m.MovieId, m.Cover, m.Title)).ToArray()
                }).ToArray();
            }

        }));
        task.Add(Task.Run(async () =>
        {
            List<DirectorRelationModel> directors = await CacheContext.Directors.SelectAsync();
            if (directors is not null && directors.Any())
            {
                relations.Directors = directors.Select(d => new MovieCounter
                {
                    Name = d.Name,
                    Movies = d.Movies.Select(m => new MovieBasic(m.MovieId, m.Cover, m.Title)).ToArray()
                }).ToArray();
            }
        }));
        await Task.WhenAll(task);

        return relations;
    }

    private void UpdateCache()
    {
        Task.Run(async () =>
        {
            bool needUpdate = await GetShouldUpdate();
            if (needUpdate)
            {
                IEnumerable<ListCard> toUpdate = await Client.GetMovies();
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
                IEnumerable<MovieRelationDto> toUpdate = await Client.GetRelativesAsync();

                MoviesRelations relations = CreateMoviesRelations(toUpdate);
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
