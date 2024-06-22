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

    public async Task Handle(RelativesDto query)
    {
        IEnumerable<Movie> relatives;
        Func<Movie, IEnumerable<string>> selector;
        string[] data = query.Data.ToArray();

        switch (query.RelativeOf)
        {
            case RelativeType.ACTOR:
                relatives = await Repository.GetAllByActors(data);
                selector = r => r.Actors;
                break;
            case RelativeType.DIRECTOR:
                relatives = await Repository.GetAllByDirectors(data);
                selector = r => r.Directors;
                break;
            case RelativeType.CATEGORY:
            default:
                relatives = await Repository.GetAllByCategories(data);
                selector = r => r.Categories;
                break;
        }

        await Output.Handle(relatives.Select(m => new RelativeMovie
        {
            Id = m.Id,
            Title = m.Title,
            Cover = m.Cover,
            Data = new RelativesDto
            {
                RelativeOf = query.RelativeOf,
                Data = selector(m).Intersect(query.Data).ToArray()
            }
        }));
    }
}
