using Consul.AspNetCore;
using Sail.Api.V1;
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

builder.Services.AddConsul(o =>
{
    o.Address = new Uri("http://127.0.0.1:8500");
});

builder.Services.AddGrpcClient<ClusterService.ClusterServiceClient>(o => o.Address = new Uri("http://localhost:8000"));
builder.Services.AddGrpcClient<RouteService.RouteServiceClient>(o => o.Address = new Uri("http://localhost:8000"));
builder.Services.AddGrpcClient<CertificateService.CertificateServiceClient>(o => o.Address = new Uri("http://localhost:8000"));


var app = builder.Build();

app.MapReverseProxy();
await app.RunAsync();