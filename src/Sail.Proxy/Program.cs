using Sail.Compass.Management;
using Sail.Core.Management;
using Yarp.Extensions.Resilience.ServiceDiscovery;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseCertificateSelector();

builder.Services.AddSailCore();
builder.Services.AddServerCertificateSelector();
builder.Services.AddServiceDiscovery()
    .AddConsulSrvServiceEndpointProvider();

builder.Services.AddControllerRuntime();
builder.Services.AddReverseProxy().LoadFromMessages()
    .AddServiceDiscoveryDestinationResolver();

var app = builder.Build();

app.MapReverseProxy();
await app.RunAsync();