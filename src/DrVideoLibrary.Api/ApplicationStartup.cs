[assembly: FunctionsStartup(typeof(ApplicationStartup))]
namespace DrVideoLibrary.Api;

internal class ApplicationStartup : FunctionsStartup
{
    //public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    //{
    //    base.ConfigureAppConfiguration(builder);
    //    var context = builder.GetContext();
    //    //builder.ConfigurationBuilder.AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), true, false);
    //    //builder.ConfigurationBuilder.AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.Development.json"), true, false);
    //    builder.ConfigurationBuilder.AddEnvironmentVariables();
    //}

    public override void Configure(IFunctionsHostBuilder builder)
    {
        IConfiguration configuration = builder.GetContext().Configuration;
        builder.Services.AddApplicationOptions(
            database => configuration.Bind(database),
            storage => configuration.Bind(storage));
        builder.Services.AddBlobStorageServices();
        builder.Services.AddUseCases();
    }


}
