using DrVideoLibrary.Razor.Options;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddServices(
    apiOptions: builder.Configuration.GetSection(ApiClientOptions.SectionKey).Bind,
    paginatorOptions: builder.Configuration.GetSection(PaginatorOptions.SectionKey).Bind);
//builder.Services.AddServices(options => builder.Configuration.Bind("Local", options.ProviderOptions));   
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
await builder.Build().RunAsync();
