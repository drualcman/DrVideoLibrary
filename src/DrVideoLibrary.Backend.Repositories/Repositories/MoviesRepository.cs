namespace DrVideoLibrary.Backend.Repositories.Repositories;
internal class MoviesRepository : IMoviesRepository
{
    readonly IMoviesContext Context;

    public MoviesRepository(IMoviesContext context)
    {
        Context = context;
    }

    public async Task AddMovie(Movie data)
    {
        MovieModel model = MovieModel.FromMovie(data);
        await Context.AddMovie(model);
    }

    public async Task<IEnumerable<Movie>> GetAll()
    {
        IEnumerable<MovieModel> movies = await Context.GetMoviesAll();
        return movies.Select(m => m.ToMovie()).OrderBy(m => m.Title);
    }
    public async Task<IEnumerable<Movie>> GetAllByActors(string[] actors)
    {
        IEnumerable<MovieModel> movies = await Context.GetMoviesAllByActors(actors);
        return movies.Select(m => m.ToMovie());
    } 
    public async Task<IEnumerable<Movie>> GetAllByDirectors(string[] directors)
    {
        IEnumerable<MovieModel> movies = await Context.GetMoviesAllByDirectors(directors);
        return movies.Select(m => m.ToMovie());
    } 
    public async Task<IEnumerable<Movie>> GetAllByCategories(string[] categories)
    {
        IEnumerable<MovieModel> movies = await Context.GetMoviesAllByCategories(categories);
        return movies.Select(m => m.ToMovie()); 
    }

    public async Task<Movie> GetMovieById(string id)
    {
        MovieModel movie = await Context.GetMovieById(id);
        return movie.ToMovie();
    }

    public async Task<int> GetTotalViews(string movieId)
    {
        IEnumerable<RegisterView> moviesWatched = await Context.GetTotalViews(movieId);
        return moviesWatched.Count();
    }

    public async Task<WatchingNowDto> GetWatchingNow()
    {
        RegisterView registerView = await Context.GetWatchingNow();
        return new WatchingNowDto
        {
            MovieId = registerView.MovieId,
            Start = registerView.Start
        };
    }

    public async Task RegisterWatchingNow(WatchingNowDto now)
    {
        RegisterView data = new RegisterView(now.MovieId, now.Start);
        await Context.RegisterWatchingNow(data);
    }

    public async Task<IEnumerable<WatchedCard>> GetWatchList()
    {
        List<WatchedModel> movies = new();
        List<WatchedCountModel> views = new();
        List<Task> tasks = new List<Task>
        {
            Task.Run(async () => movies = new(await Context.GetWatchedMoviesAll())),
            Task.Run(async () => views = new(await Context.GetWatchedViewsAll()))
        };
        await Task.WhenAll(tasks);
        return movies.GroupJoin(
            views,
            movie => movie.Id,
            view => view.MovieId,
            (movie, viewGroup) => new
            {
                Movie = movie,
                Views = viewGroup.FirstOrDefault()
            }
        )
        .Where(x => x.Views != null)
        .Select(x => new WatchedCard(
            x.Movie.Id,
            x.Movie.Title,
            x.Movie.Cover,
            x.Movie.Duration,
            x.Movie.Rate,
            x.Views.Times
        ))
        .OrderBy(v => v.Times)
        .ThenBy(m => m.Title);
    }
}
