namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyContainer
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<IGetRelativesController, GetRelativesController>();
        services.AddScoped<IGetRelativesInputPort, GetRelativesInteractor>();
        services.AddScoped<IGetRelativesOutputPort, GetRelativesPresenter>();

        services.AddScoped<ISeachMoviesController, SeachMoviesController>();
        services.AddScoped<ISeachMoviesInputPort, SeachMoviesInteractor>();

        services.AddScoped<ISearchMoveDetailController, SearchMoveDetailController>();
        services.AddScoped<ISearchMoveDetailInputPort, SearchMoveDetailInteractor>();

        services.AddScoped<IAddMovieController, AddMovieController>();
        services.AddScoped<IAddMovieInputPort, AddMovieInteractor>();

        services.AddScoped<IGetAllController, GetAllController>();
        services.AddScoped<IGetAllInputPort, GetAllInteractor>();
        services.AddScoped<IGetAllOutputPort, GetAllPresenter>(); 

        services.AddScoped<IRegisterWatchingNowController, RegisterWatchingNowController>();
        services.AddScoped<IRegisterWatchingNowInputPort, RegisterWatchingNowInteractor>();

        services.AddScoped<IGetWhatchingNowController, GetWhatchingNowController>();
        services.AddScoped<IGetWhatchingNowInputPort, GetWhatchingNowInteractor>();
        services.AddScoped<IGetWhatchingNowOutputPort, GetWhatchingNowPresenter>();
        return services;
    }

}
