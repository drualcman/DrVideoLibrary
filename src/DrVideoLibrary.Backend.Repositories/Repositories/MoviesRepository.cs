using DrVideoLibrary.Entities.ValueObjects;

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

    public async Task<IEnumerable<ListCard>> GetAll()
    {
        IEnumerable<MovieModel> movies = await context.GetAll();
        return movies.Select(m=> new ListCard(m.Id, m.Title, m.Cover, m.Year, m.Categories));
    }
}
