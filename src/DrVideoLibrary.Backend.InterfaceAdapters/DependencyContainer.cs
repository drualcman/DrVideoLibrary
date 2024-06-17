namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyContainer
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<IGetRelativesController, GetRelativesController>();
        services.AddScoped<IGetRelativesOutputPort, GetRelativesPresenter>();
        services.AddScoped<IGetRelativesInputPort, GetRelativesInteractor>();
        
        services.AddScoped<ISeachMoviesController, SeachMoviesController>();
        services.AddScoped<ISeachMoviesInputPort, SeachMoviesInteractor>();

        services.AddScoped<ISearchMoveDetailController, SearchMoveDetailController>();
        services.AddScoped<ISearchMoveDetailInputPort, SearchMoveDetailInteractor>();
        return services;
    }

}
