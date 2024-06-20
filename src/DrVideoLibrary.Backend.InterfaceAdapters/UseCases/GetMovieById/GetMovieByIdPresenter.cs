namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.GetMovieById;
internal class GetMovieByIdPresenter : IGetMovieByIdOutputPort
{
    readonly IFileContent FileContent;

    public GetMovieByIdPresenter(IFileContent fileContent, Movie content)
    {
        FileContent = fileContent;
        Content = content;
    }

    public Movie Content { get;private set; }

    public async Task Handle(Movie data)
    {
        if(data is not null)
        {
            Uri uri = await FileContent.GetUri(data.Cover);
            data.Cover = uri.ToString();
        }
        Content = data;
    }
}
