namespace DrVideoLibrary.Razor.Cache.Services;
internal class MoviesCacheService(MoviesContext CacheContext, ApiClient Client, IJSRuntime JsRuntime)
{
    MoviesRelations RelativeMovies;
    public async Task ResetCache()
    {
        UpdateCacheOnGoing = false;
        await CacheContext.DropDatabaseAsync();
        await CacheContext.Init();
    }

    public async Task<IEnumerable<ListCard>> GetList()
    {
        IEnumerable<ListCard> movies = [];
        IEnumerable<ListCard> firstData = null;
        List<MovieCardModel> cached = await CacheContext.Movies.SelectAsync();
        if(cached is not null && cached.Any())
        {
            movies = cached.Select(MovieCardModel.ToListCard);
        }
        else
        {
            movies = await Client.GetMovies();
            firstData = movies;
        }
        await UpdateCache(firstData);
        return movies.OrderBy(m => m.Title).ThenBy(m => m.Year);
    }

    public void ProcessRelatives()
    {
        if(RelativeMovies is null ||
           RelativeMovies.Categories is null ||
           RelativeMovies.Actors is null ||
           RelativeMovies.Directors is null)
        {
            Task.Run(async () =>
            {
                RelativeMovies = await GetRelations();
            });
        }
    }

    public MovieCounter[] GetRelatives(RelativeType type) =>
        type switch
        {
            RelativeType.ACTOR => RelativeMovies?.Actors ?? [],
            RelativeType.DIRECTOR => RelativeMovies?.Directors ?? [],
            _ => RelativeMovies?.Categories ?? [],
        };

    public async Task<IEnumerable<RelativeMovie>> GetRelativesAsync(RelativesDto data) =>
        data.RelativeOf switch
        {
            RelativeType.ACTOR => await GetMoviesByRelation(RelativeMovies?.Actors, data),
            RelativeType.DIRECTOR => await GetMoviesByRelation(RelativeMovies?.Directors, data),
            _ => await GetMoviesByRelation(RelativeMovies?.Categories, data),
        };

    private async Task<List<RelativeMovie>> GetMoviesByRelation(MovieCounter[] relations, RelativesDto data)
    {
        ConcurrentDictionary<string, RelativeMovie> moviesDict = new();
        if(data is not null)
        {
            List<Task> tasks = new();

            foreach(string name in data.Data)
            {
                tasks.Add(Task.Run(() =>
                {
                    MovieCounter relation = relations?.FirstOrDefault(r => r.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) ?? false);
                    if(relation != null)
                    {
                        foreach(var movie in relation.Movies)
                        {
                            if(!moviesDict.ContainsKey(movie.Id))
                            {
                                moviesDict.TryAdd(movie.Id, new RelativeMovie
                                {
                                    Id = movie.Id,
                                    Cover = movie.Cover,
                                    Title = movie.Title,
                                    Data = new RelativesDto
                                    {
                                        RelativeOf = data.RelativeOf,
                                        Data = [name]
                                    }
                                });
                            }
                            else
                            {
                                RelativeMovie existingMovie = moviesDict[movie.Id];
                                existingMovie.Data.Data = existingMovie.Data.Data.Append(name).Order().ToArray();
                            }
                        }
                    }
                }));
            }
            await Task.WhenAll(tasks);
        }
        return moviesDict.Values.OrderBy(m => m.Title).ToList();
    }

    private async Task<MoviesRelations> GetRelations()
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
            }).OrderByDescending(o => o.Count).ThenBy(n => n.Name).ToArray();
        }));
        task.Add(Task.Run(async () =>
        {
            List<ActorRelationModel> actors = await CacheContext.Actors.SelectAsync();
            if(actors is not null && actors.Any())
            {
                relations.Actors = actors.Select(a => new MovieCounter
                {
                    Name = a.Name,
                    Movies = a.Movies.Select(m => new MovieBasic(m.MovieId, m.Cover, m.Title)).ToArray()
                }).OrderByDescending(o => o.Count).ThenBy(n => n.Name).ToArray();
            }

        }));
        task.Add(Task.Run(async () =>
        {
            List<DirectorRelationModel> directors = await CacheContext.Directors.SelectAsync();
            if(directors is not null && directors.Any())
            {
                relations.Directors = directors.Select(d => new MovieCounter
                {
                    Name = d.Name,
                    Movies = d.Movies.Select(m => new MovieBasic(m.MovieId, m.Cover, m.Title)).ToArray()
                }).OrderByDescending(o => o.Count).ThenBy(n => n.Name).ToArray();
            }
        }));
        await Task.WhenAll(task);
        return relations;
    }

    bool UpdateCacheOnGoing;
    private async Task UpdateCache(IEnumerable<ListCard> firstData)
    {
        if(!UpdateCacheOnGoing)
        {
            UpdateCacheOnGoing = true;
            List<Task> tasks = [];
            tasks.Add(UpdateCacheData(firstData));
            tasks.Add(UpdateExpiredCacheData());
            await Task.WhenAll(tasks);
            UpdateCacheOnGoing = false;
        }
    }

    private async Task UpdateCacheData(IEnumerable<ListCard> firstData)
    {
        bool needUpdate = await GetShouldUpdate();
        if(needUpdate)
        {
            IEnumerable<ListCard> toUpdate = firstData ?? await Client.GetMovies();
            await CacheContext.Movies.CleanAsync();
            await CacheContext.Movies.AddAsync(toUpdate.Select(MovieCardModel.FromListCard).ToList());
            await JsRuntime.InvokeVoidAsync("localStorage.setItem", "last-update", DateTime.UtcNow);
        }
    }

    private async Task UpdateExpiredCacheData()
    {
        DateTime actual = DateTime.Today;
        DateTime last = actual.AddYears(-1);
        try
        {
            DateTime? lastUpdate = await JsRuntime.InvokeAsync<DateTime?>("localStorage.getItem", "last-relations-update");
            if(lastUpdate.HasValue)
                last = lastUpdate.Value;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        TimeSpan difference = actual.Subtract(last);
        int daysPassed = (int)difference.TotalDays;
        bool needUpdate = daysPassed > 30;
        if(needUpdate)
        {
            IEnumerable<MovieRelationDto> toUpdate = await Client.GetRelativesAsync();

            MoviesRelations relations = await CreateMoviesRelations(toUpdate);
            List<Task> updatedbTask =
            [
                Task.Run(async () =>
                        {
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
                        }),
                        Task.Run(async () =>
                        {
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
                        })
            ];
            await Task.WhenAll(updatedbTask);
            await JsRuntime.InvokeVoidAsync("localStorage.setItem", "last-relations-update", DateTime.UtcNow);
        }
    }

    private async Task<MoviesRelations> CreateMoviesRelations(IEnumerable<MovieRelationDto> moviesDetails)
    {
        Dictionary<string, List<MovieBasic>> actorMovies = new Dictionary<string, List<MovieBasic>>();
        Dictionary<string, List<MovieBasic>> directorMovies = new Dictionary<string, List<MovieBasic>>();

        foreach(MovieRelationDto movie in moviesDetails)
        {
            MovieBasic movieBasic = new MovieBasic(movie.Id, movie.Cover, movie.Title);
            List<Task> tasks =
            [
                Task.Run(() =>
                {
                    foreach (string actor in movie.Actors)
                    {
                        if (!actorMovies.ContainsKey(actor))
                            actorMovies[actor] = new List<MovieBasic>();
                        actorMovies[actor].Add(movieBasic);
                    }
                }),
                Task.Run(() =>
                {
                    foreach (string director in movie.Directors)
                    {
                        if (!directorMovies.ContainsKey(director))
                            directorMovies[director] = new List<MovieBasic>();
                        directorMovies[director].Add(movieBasic);
                    }
                })
            ];
            await Task.WhenAll(tasks);
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
            if(!result)
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
            if(lastUpdate.HasValue)
            {
                DateOnly last = DateOnly.FromDateTime(lastUpdate.Value);
                DateOnly today = DateOnly.FromDateTime(DateTime.Today);
                result = last != today;
            }
            else
                result = true;
        }
        catch
        {
            result = true;
        }
        return result;
    }
}
