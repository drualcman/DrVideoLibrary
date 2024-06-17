namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.AddMovie;
internal class AddMovieInteractor : IAddMovieInputPort
{
    readonly IServiceScopeFactory ScopeFactory;

    public AddMovieInteractor(IServiceScopeFactory scopeFactory)
    {
        ScopeFactory = scopeFactory;
    }

    public Task AddMovie(Movie data)
    {
        Task.Run(async () =>
        {
            using AsyncServiceScope scope = ScopeFactory.CreateAsyncScope();
            IFileContent fileManager = scope.ServiceProvider.GetRequiredService<IFileContent>();
            IMoviesRepository moviesRepository = scope.ServiceProvider.GetRequiredService<IMoviesRepository>();

            data.Id = Guid.NewGuid().ToString();
            if (string.IsNullOrEmpty(data.Cover) == false)
            {
                byte[] bytes = await fileManager.GetUrlBytesAsync(data.Cover);
                string filename = Path.GetFileName(data.Cover);
                filename = await fileManager.UploadFile(bytes, filename, data.Id);
                data.Cover = filename;
            }
            await moviesRepository.AddMovie(data);
        });
        return Task.CompletedTask;
    }
}
