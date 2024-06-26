namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.AddMovie;
internal class AddMovieInteractor : IAddMovieInputPort
{
    readonly IServiceScopeFactory ScopeFactory;
    readonly IEventHub<SendNotificationSubscription> EventHub;

    public AddMovieInteractor(IServiceScopeFactory scopeFactory, IEventHub<SendNotificationSubscription> eventHub)
    {
        ScopeFactory = scopeFactory;
        EventHub = eventHub;
    }

    public Task AddMovie(Movie data, ILogger logger)
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
            await EventHub.Rise(new SendNotificationSubscription(
                $"Tengo una peli nueva!",
                data.Id,
                ApplicationBusinessRules.ValueObjects.SendNotificationType.CATALOG), logger);
        });
        return Task.CompletedTask;
    }
}
