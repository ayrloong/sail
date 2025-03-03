using Consul.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConsul(o =>
{
    // Consul地址.如果非本地请修改
    o.Address = new("http://127.0.0.1:8500");
}).AddConsulServiceRegistration(registrationConfig =>
{
    registrationConfig.Meta = new Dictionary<string, string>() { { "Weight", "1" } };
    registrationConfig.ID = "SVC1";
    registrationConfig.Port = 5124;
    registrationConfig.Name = "test";
    registrationConfig.Address = "http://127.0.0.1";
    registrationConfig.Tags = ["SampleServer"];
});
var app = builder.Build();


app.MapGet("/", () => "Hello World!");

app.Run();