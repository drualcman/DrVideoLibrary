var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddServices();
//builder.Services.AddServices(options => builder.Configuration.Bind("Local", options.ProviderOptions));   
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
await builder.Build().RunAsync();
