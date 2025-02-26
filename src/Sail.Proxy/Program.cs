using Sail.Compass.Management;
using Sail.Core.Management;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseCertificateSelector();

builder.Services.AddSailCore();
builder.Services.AddServerCertificateSelector();
builder.Services.AddServiceDiscovery()
    .AddConsulSrvServiceEndpointProvider();
builder.Services.AddControllerRuntime();
builder.Services.AddReverseProxy().LoadFromMessages();

var app = builder.Build();

app.MapReverseProxy();
app.Run();