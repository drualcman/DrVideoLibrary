namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyContainer
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<IGetRelativesController, GetRelativesController>();
        services.AddScoped<IGetRelativesInputPort, GetRelativesInteractor>();
        services.AddScoped<IGetRelativesOutputPort, GetRelativesPresenter>();
        return services;
    }

}
