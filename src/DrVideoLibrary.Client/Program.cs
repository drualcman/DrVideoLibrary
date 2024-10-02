var builder = WebAssemblyHostBuilder.CreateDefault(args);


builder.Services.AddServices(
    apiOptions: builder.Configuration.GetSection(ApiClientOptions.SectionKey).Bind,
    paginatorOptions: builder.Configuration.GetSection(PaginatorOptions.SectionKey).Bind,
    searchMovieOptions: builder.Configuration.GetSection(SearchMovieOptions.SectionKey).Bind,
    authOptions: options => builder.Configuration.GetSection("Local").Bind(options.ProviderOptions),
    cacheDbOptions: builder.Configuration.GetSection(CacheDbOptions.SectionKey).Bind,
    pushNotificationOptions: builder.Configuration.GetSection(PushNotificationOptions.SectionKey).Bind);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var host = builder.Build();
await host.SetDefaultCulture();
await host.RunAsync();