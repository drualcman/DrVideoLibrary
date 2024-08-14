namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.GetRelatives;
internal class GetRelativesInteractor : IGetRelativesInputPort
{
    readonly IGetRelativesOutputPort Output;
    readonly IMoviesRepository Repository;

    public GetRelativesInteractor(IGetRelativesOutputPort output, IMoviesRepository repository)
    {
        Output = output;
        Repository = repository;
    }

    public async Task Handle()
    {
        IEnumerable<Movie> relatives = await Repository.GetAll();
        await Output.Handle(relatives.Select(m => new MovieRelationDto
        {
            Id = m.Id,
            Title = m.Title,
            Cover = m.Cover,
            Directors = m.Directors,
            Actors = m.Actors
        }));
    }
}
