namespace DrVideoLibrary.Backend.Repositories.Repositories;
internal class MoviesRepository : IMoviesRepository
{
    readonly IMoviesContext context;

    public MoviesRepository(IMoviesContext context)
    {
        this.context = context;
    }

    public async Task AddMovie(Movie data)
    {
        MovieModel model = MovieModel.FromMovie(data);
        await context.AddMovie(model);
    }

    public async Task<IEnumerable<Movie>> GetAll()
    {
        IEnumerable<MovieModel> movies = await context.GetAll();
        return movies.Select(m => m.ToMovie());
    }

    public Task<Movie> GetById(string id)
    {
        return Task.FromResult(new Movie { Id = id });
    }

    public async Task RegisterWatchingNow(string id)
    {
        RegisterView data = new RegisterView(id, DateTime.UtcNow);
        await context.RegisterWatchingNow(data);
    }
}
