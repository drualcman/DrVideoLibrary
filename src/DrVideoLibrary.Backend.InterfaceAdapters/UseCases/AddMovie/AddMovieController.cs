namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.AddMovie;
internal class AddMovieController : IAddMovieController
{
    readonly IAddMovieInputPort Input;

    public AddMovieController(IAddMovieInputPort input)
    {
        Input = input;
    }

    public Task AddMovie(Movie data) =>
        Input.AddMovie(data);
}
