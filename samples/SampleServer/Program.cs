using Consul.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

builder.Services.AddConsul(o =>
{
    o.Address = new Uri("http://127.0.0.1:8500");
}).AddConsulServiceRegistration(registrationConfig =>
{
    registrationConfig.Meta = new Dictionary<string, string> { { "Weight", "1" } };
    registrationConfig.ID = "SVC1";
    registrationConfig.Port = 5132;
    registrationConfig.Name = "test";
    registrationConfig.Address = "http://127.0.0.1";
    registrationConfig.Tags = ["SampleServer"];
    registrationConfig.Check = new Consul.AgentServiceCheck
    {
        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(15),
        Interval = TimeSpan.FromSeconds(15),
        HTTP = "http://127.0.0.1:5132/health",
        Timeout = TimeSpan.FromSeconds(5),
        Method = "GET",
    };
});
var app = builder.Build();


app.MapGet("/", () => "Hello World!");
app.UseHealthChecks("/health");

app.Run();