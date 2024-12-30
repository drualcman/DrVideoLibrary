namespace DrVideoLibrary.Razor.Components;
public partial class MovieDetailComponent
{
    [Inject] IStringLocalizer<ResourceMovieDetailComponent> Localizer { get; set; }
    [Parameter] public Movie Movie { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; }
    [Parameter] public EventCallback OnPlayClick { get; set; }

    RelativesDto RelativesData;
    string Actor = string.Empty;

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            if (Movie is not null)
            {
                Task.Run(async () =>
                {
                    RelativesData = new RelativesDto
                    {
                        RelativeOf = RelativeType.ACTOR,
                        Data = Movie.Actors.ToArray()
                    };
                    await InvokeAsync(StateHasChanged);
                });
            }
        }
    }

    //protected override void OnParametersSet()
    //{
    //    if (Movie is not null)
    //    {
    //        RelativesData = new RelativesDto
    //        {
    //            RelativeOf = RelativeType.ACTOR,
    //            Data = Movie.Actors.ToArray()
    //        };
    //    }
    //}

    Task ChangeRelative(RelativeType relativeType)
    {
        RelativesData.RelativeOf = relativeType;
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