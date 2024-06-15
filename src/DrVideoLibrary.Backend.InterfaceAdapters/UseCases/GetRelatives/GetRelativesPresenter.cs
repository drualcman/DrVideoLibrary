namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.GetRelatives;
internal class GetRelativesPresenter : IGetRelativesOutputPort
{
    readonly IFileContent FileContent;

    public GetRelativesPresenter(IFileContent fileContent)
    {
        FileContent = fileContent;
    }

    public IEnumerable<RelativeMovie> Content { get; private set; }

    public async Task Handle(IEnumerable<RelativeMovie> data)
    {
        await Parallel.ForEachAsync(data, async (relativeMovie, cancellationToken) =>
        {
            if (!relativeMovie.Cover.ToLower().Contains("http"))
            {
                Uri uri = await FileContent.GetUri(relativeMovie.Cover);
                relativeMovie.Cover = uri.ToString();
            }
        });
        Content = data;
    }
}
