namespace Sail.Dashboard.Models.Routes;

public class RouteResult
{
    public Guid Id { get; set; }
    public Guid? ClusterId { get; set; }
    public string? ClusterName { get; set; }
    public required string Name { get; set; }
    public RouteMatch? Match { get; set; }
}