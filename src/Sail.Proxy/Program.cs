using Consul.AspNetCore;
using Sail.Compass.Management;
using Sail.Core.Management;
using Sail.Core.Options;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseCertificateSelector();

builder.Services.AddSailCore();
builder.Services.AddServerCertificateSelector();
builder.Services.AddServiceDiscovery()
    .AddConsulSrvServiceEndpointProvider();

builder.Services.AddControllerRuntime();
builder.Services.AddReverseProxy().LoadFromMessages()
    .AddServiceDiscoveryDestinationResolver();

builder.Services.AddConsul(o =>
{
    o.Address = new Uri("http://127.0.0.1:8500");
});

var app = builder.Build();

app.MapReverseProxy();
await app.RunAsync();