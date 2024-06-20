
namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.GetMovieById;
internal class GetMovieByIdInteractor : IGetMovieByIdInputPort
{
    readonly IMoviesRepository Repository;
    readonly IGetMovieByIdOutputPort Output;

    public GetMovieByIdInteractor(IMoviesRepository repository, IGetMovieByIdOutputPort output)
    {
        Repository = repository;
        Output = output;
    }

    public async Task Handle(string id)
    {
        Movie data = await Repository.GetMovieById(id);
        await Output.Handle(data);  
    }
}
