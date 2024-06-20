namespace DrVideoLibrary.Razor.Components;
public partial class MovieDetailComponent
{
    [Inject] IStringLocalizer<ResourceMovieDetailComponent> Localizer { get; set; }
    [Parameter] public Movie Movie { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; }

    GetRelativesDto RelativesData;

    protected override void OnParametersSet()
    {
        if (Movie is not null)
        {
            RelativesData = new GetRelativesDto
            {
                RelativeOf = RelativeType.CATEGORY,
                Data = Movie.Categories.ToArray()
            };
        }
    }

    Task ChangeRelative(RelativeType relativeType)
    {
        RelativesData = new GetRelativesDto
        {
            RelativeOf = relativeType
        };
        switch (relativeType)
        {
            case RelativeType.ACTOR:
                RelativesData.Data = Movie.Actors.ToArray();
                break;
            case RelativeType.DIRECTOR:
                RelativesData.Data = Movie.Directors.ToArray();
                break;
            case RelativeType.CATEGORY:
            default:
                RelativesData.Data = Movie.Categories.ToArray();
                break;
        }
        return Task.CompletedTask;
    }

}