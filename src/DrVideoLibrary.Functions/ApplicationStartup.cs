//[assembly: FunctionsStartup(typeof(ApplicationStartup))]
//namespace DrVideoLibrary.Functions;

//internal class ApplicationStartup : FunctionsStartup
//{
//    public override void Configure(IFunctionsHostBuilder builder)
//    {
//        IConfiguration configuration = builder.GetContext().Configuration;
//        builder.Services.AddApplicationServices(
//            database => configuration.Bind(database),
//            spanish => configuration.Bind(spanish),
//            english => configuration.Bind(english),
//            translation => configuration.Bind(translation),
//            storage => configuration.Bind(storage),
//            notification => configuration.Bind(notification));

//        builder.Services.AddBlobStorageServices();
//        builder.Services.AddContextServices();
//        builder.Services.AddBackendServices();
//        builder.Services.AddPushNotifications();
//        builder.Services.AddUseCases();
//    }
//}
