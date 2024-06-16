using DrVideoLibrary.Razor.Options;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddServices(
    apiOptions: builder.Configuration.GetSection(ApiClientOptions.SectionKey).Bind,
    paginatorOptions: builder.Configuration.GetSection(PaginatorOptions.SectionKey).Bind,
    translationOptions: builder.Configuration.GetSection(TranslationOptions.SectionKey).Bind,
    searchMovieOptions: builder.Configuration.GetSection(SearchMovieOptions.SectionKey).Bind,
    authOptions: options => builder.Configuration.GetSection("Local").Bind(options.ProviderOptions));   
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
await builder.Build().RunAsync();
