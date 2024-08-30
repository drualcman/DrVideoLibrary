namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.GetRelatives;
internal class GetRelativesPresenter : IGetRelativesOutputPort
{
    private readonly IFileContent FileContent;

    public GetRelativesPresenter(IFileContent fileContent)
    {
        FileContent = fileContent;
    }

    public IEnumerable<MovieRelationDto> Content { get; protected set; }

    public async Task Handle(IEnumerable<MovieRelationDto> data)
    {
        ConcurrentBag<MovieRelationDto> result = new ConcurrentBag<MovieRelationDto>();
        IEnumerable<Task> tasks = data.Select(async item =>
        {
            await UpdateCoverAsync(item);
            result.Add(item);
        });
        await Task.WhenAll(tasks);
        Content = result;
    }

    private async Task UpdateCoverAsync(MovieRelationDto movie)
    {
        if (!movie.Cover.ToLower().Contains("http"))
        {
            Uri uri = await FileContent.GetUri(movie.Cover, 30);
            movie.Cover = uri.ToString();
        }
    }
}
