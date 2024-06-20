using DrVideoLibrary.Entities.Dtos;

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
        IEnumerable<MovieModel> movies = await Context.GetAll();
        return movies.Select(m => m.ToMovie());
    }
    public async Task<IEnumerable<Movie>> GetAllByActors(string[] actors)
    {
        IEnumerable<MovieModel> movies = await Context.GetAllByActors(actors);
        return movies.Select(m => m.ToMovie());
    } 
    public async Task<IEnumerable<Movie>> GetAllByDirectors(string[] directors)
    {
        IEnumerable<MovieModel> movies = await Context.GetAllByDirectors(directors);
        return movies.Select(m => m.ToMovie());
    } 
    public async Task<IEnumerable<Movie>> GetAllByCategories(string[] categories)
    {
        IEnumerable<MovieModel> movies = await Context.GetAllByCategories(categories);
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
}
