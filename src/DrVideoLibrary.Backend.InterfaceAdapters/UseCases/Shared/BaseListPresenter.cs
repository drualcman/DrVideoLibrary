namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.Shared;
internal class BaseListPresenter<T> where T : IMovie
{
    protected readonly IFileContent FileContent;

    protected BaseListPresenter(IFileContent fileContent)
    {
        FileContent = fileContent;
    }

    public IEnumerable<T> Content { get; protected set; }

    public async Task Handle(IEnumerable<T> data)
    {
        var result = new ConcurrentBag<T>();
        var tasks = data.Select(async item =>
        {
            await UpdateCoverAsync(item);
            result.Add(item);
        });
        await Task.WhenAll(tasks);
        Content = result;
    }

    private async Task UpdateCoverAsync(T movie)
    {
        if (!movie.Cover.ToLower().Contains("http"))
        {
            Uri uri = await FileContent.GetUri(movie.Cover);
            movie.Cover = uri.ToString();
        }
    }
}
