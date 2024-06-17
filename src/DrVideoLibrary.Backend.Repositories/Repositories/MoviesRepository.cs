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
}
