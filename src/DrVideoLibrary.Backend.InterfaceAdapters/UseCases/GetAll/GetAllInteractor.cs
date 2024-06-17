
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
        IEnumerable<Movie> data = await Repository.GetAll();
        await Output.Handle(data.Select(m=> new ListCard(m.Id, m.Title, m.Cover, m.Year, m.Categories)));
    }
}
