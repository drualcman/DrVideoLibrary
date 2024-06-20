using DrVideoLibrary.Entities.Models;

namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.GetRelatives;
internal class GetRelativesInteractor: IGetRelativesInputPort
{
    readonly IGetRelativesOutputPort Output;
    readonly IMoviesRepository Repository;

    public GetRelativesInteractor(IGetRelativesOutputPort output, IMoviesRepository repository)
    {
        Output = output;
        Repository = repository;
    }

    public async Task Handle(GetRelativesDto query)
    {
        IEnumerable<Movie> relatives;
        switch (query.RelativeOf)
        {
            case RelativeType.ACTOR:
                relatives = await Repository.GetAllByActors(query.Data);
                break;
            case RelativeType.DIRECTOR:
                relatives = await Repository.GetAllByDirectors(query.Data);
                break;
            case RelativeType.CATEGORY:
            default:
                relatives = await Repository.GetAllByCategories(query.Data);
                break;
        }
        await Output.Handle(relatives.Select(m => new RelativeMovie
        {
            Id = m.Id,
            Title = m.Title,
            Cover = m.Cover
        }));
    }
}
