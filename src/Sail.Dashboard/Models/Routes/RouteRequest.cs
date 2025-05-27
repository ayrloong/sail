namespace Sail.Dashboard.Models.Routes;

public class RouteRequest
{
    public string? ClusterId { get; set; }
    public string Name { get; set; }
    public RouteMatch? Match { get; set; }
}