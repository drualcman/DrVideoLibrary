
namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.GetWhatchingNow;
internal class GetWhatchingNowPresenter : IGetWhatchingNowOutputPort
{
    readonly IFileContent FileContent;

    public GetWhatchingNowPresenter(IFileContent fileContent, WatchingNow content)
    {
        FileContent = fileContent;
        Content = content;
    }

    public WatchingNow Content { get; private set; }    

    public async Task Handle(Movie movie, DateTime started)
    {
        Content = new WatchingNow
        {
            Movie = movie,
            Start = started,
        };
        if (string.IsNullOrEmpty(movie.Cover) == false)
        {
            Uri uri = await FileContent.GetUri(movie.Cover);
            Content.Movie.Cover = uri.ToString();
        }
    }
}
