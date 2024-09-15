
namespace DrVideoLibrary.Razor.Pages;
public partial class Relations
{
    [Inject] IStringLocalizer<ResourceRelations> Localizer { get; set; }
    [Inject] MoviesCacheService CacheService { get; set; }

    MovieCounter[] MoviesRelations;
    MovieCounter Selection;
    string Actor = string.Empty;
    private RelativeType RelativeToBF;
    PaginationObjectHandler<MovieCounter> Paginator;
    private string SearchBK;

    public string Search
    {
        get { return SearchBK; }
        set 
        { 
            SearchBK = value;
            try
            {
                MoviesRelations = CacheService.GetRelatives(RelativeToBF);
                if (MoviesRelations is not null)
                {
                    if (!string.IsNullOrEmpty(SearchBK))
                    {
                        MoviesRelations = MoviesRelations
                            .Where(x => x.Name.Contains(SearchBK, StringComparison.InvariantCultureIgnoreCase))
                            .ToArray();
                    }
                    Paginator = new PaginationObjectHandler<MovieCounter>(MoviesRelations, 10);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MoviesRelations = [];
            }
        }
    }


    public RelativeType RelativeTo
	{
		get { return RelativeToBF; }
		set 
		{ 
			RelativeToBF = value;
            Selection = null;
            SearchBK = string.Empty;
            try
            {
                MoviesRelations = CacheService.GetRelatives(RelativeToBF);
                if (MoviesRelations is not null)
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
        Actor = string.Empty;
        await InvokeAsync(StateHasChanged);
    }
}