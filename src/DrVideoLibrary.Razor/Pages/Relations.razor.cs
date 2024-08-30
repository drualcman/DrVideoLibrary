
namespace DrVideoLibrary.Razor.Pages;
public partial class Relations
{
    [Inject] IStringLocalizer<ResourceRelations> Localizer { get; set; }
    [Inject] MoviesCacheService CacheService { get; set; }

    MovieCounter[] MoviesRelations;
    MovieCounter Selection;
    private RelativeType RelativeToBF;
    PaginationObjectHandler<MovieCounter> Paginator;


    public RelativeType RelativeTo
	{
		get { return RelativeToBF; }
		set 
		{ 
			RelativeToBF = value;
            try
            {
                MoviesRelations = CacheService.GetRelatives(RelativeToBF);
                if(MoviesRelations is not null)
                    Paginator = new PaginationObjectHandler<MovieCounter>(MoviesRelations, 10);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MoviesRelations = [];
            }
        }
	}

	protected override void OnInitialized()
    {
        RelativeTo = RelativeType.CATEGORY;
    }

    async Task ToPage(int page)
    {
        Selection = null;
        await InvokeAsync(StateHasChanged);
    }
}