namespace Sail.Dashboard.Models.Routes;

public class RouteResult
{
    public Guid Id { get; set; }
    public string? ClusterId { get; set; }
    public string? ClusterName { get; set; }
    public string Name { get; set; }
    public RouteMatch? Match { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}