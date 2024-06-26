namespace DrVideoLibrary.Backend.InterfaceAdapters.UseCases.GetWatchingNow;
internal class GetWatchingNowInteractor : IGetWatchingNowInputPort
{
    readonly IMoviesRepository Repository;
    readonly IGetWatchingNowOutputPort Output;

    public GetWatchingNowInteractor(IMoviesRepository repository,
        IGetWatchingNowOutputPort output)
    {
        Repository = repository;
        Output = output;
    }

    public async Task Handle()
    {
        WatchingNowDto watchingNow = await Repository.GetWatchingNow();
        Movie movie = null;
        int totalViews = 0;
        List<Task> tasks = new List<Task>()
        {
            Task.Run(async () => movie = await Repository.GetMovieById(watchingNow.MovieId)),
            Task.Run(async () => totalViews = await Repository.GetTotalViews(watchingNow.MovieId))
        };
        await Task.WhenAll(tasks);
        movie.TotalViews = totalViews;
        await Output.Handle(movie, watchingNow.Start);
    }
}
