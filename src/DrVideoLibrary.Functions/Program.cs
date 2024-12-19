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
                    database => configuration.Bind(database),
                    spanish => configuration.Bind(spanish),
                    english => configuration.Bind(english),
                    translation => configuration.Bind(translation),
                    storage => configuration.Bind(storage),
                    notification => configuration.Bind(notification));

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


