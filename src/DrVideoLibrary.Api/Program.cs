var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

// Add services to the container.
builder.Services.AddServices();
//builder.Services.AddServices(options => builder.Configuration.Bind("Local", options.ProviderOptions));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.MapRazorPages();

app.MapFallbackToPage("/_Host");

await app.RunAsync();