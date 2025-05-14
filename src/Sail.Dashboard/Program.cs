using Sail.Dashboard;
using Sail.Dashboard.Components;
using Sail.Dashboard.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<DashboardClient>((sp, client) =>
{
    client.BaseAddress = new Uri("http://localhost:8100");
}).AddApiVersion(1.0);

builder.Services.AddTransient<ClusterService>();

var app = builder.Build();

app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();