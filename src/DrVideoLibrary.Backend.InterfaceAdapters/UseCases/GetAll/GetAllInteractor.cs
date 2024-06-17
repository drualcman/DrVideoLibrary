
namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.GetAll;
internal class GetAllInteractor : IGetAllInputPort
{
    readonly IGetAllOutputPort Output;
    readonly IMoviesRepository Repository;

    public GetAllInteractor(IGetAllOutputPort output, IMoviesRepository repository)
    {
        Output = output;
        Repository = repository;
    }

    public async Task Handle()
    {
        IEnumerable<ListCard> data = await Repository.GetAll();
        await Output.Handle(data);
    }
}
