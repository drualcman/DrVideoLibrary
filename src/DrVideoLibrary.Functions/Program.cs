using DrVideoLibrary.Backend.ApplicationBusinessRules.Options;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                // Carga user secrets si está en modo de desarrollo.
                if (context.HostingEnvironment.IsDevelopment())
                {
                    config.AddUserSecrets<Program>();
                }
            })
            .ConfigureServices((context, services) =>
            {
                // Registro de dependencias y servicios
                var configuration = context.Configuration;

                //services.AddSingleton(typeof(ILogger<>), typeof(LoggerService<>));
                services.AddLogging(builder =>
                {
                    builder.AddConsole();
                });

                services.AddApplicationServices(
                    database => configuration.GetSection(ConnectionStringsOptions.SectionKey).Bind(database),
                    spanish => configuration.GetSection(SearchMovieSpanishOption.SectionKey).Bind(spanish),
                    english => configuration.GetSection(SearchMovieEnglishOption.SectionKey).Bind(english),
                    translation => configuration.GetSection(TranslationOptions.SectionKey).Bind(translation),
                    storage => configuration.GetSection(StorageOptions.SectionKey).Bind(storage),
                    notification => configuration.GetSection(NotificationOptions.SectionKey).Bind(notification));

                services.AddBlobStorageServices();
                services.AddContextServices();
                services.AddBackendServices();
                services.AddPushNotifications();
                services.AddUseCases();
            })
            .ConfigureFunctionsWebApplication()
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders(); // Limpia configuraciones previas
                logging.AddConsole();     // Añade soporte de logging a consola
            })
            .Build();

await host.RunAsync();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();



