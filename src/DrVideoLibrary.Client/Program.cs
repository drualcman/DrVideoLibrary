var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddServices();
//builder.Services.AddServices(options => builder.Configuration.Bind("Local", options.ProviderOptions));
await builder.Build().RunAsync();
