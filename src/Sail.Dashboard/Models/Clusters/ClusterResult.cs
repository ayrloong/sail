namespace Sail.Dashboard.Models.Clusters;

public record ClusterResult
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? ServiceName { get; set; }
    public string? LoadBalancingPolicy { get; set; }
    public HealthCheck? HealthCheck { get; set; }
    public SessionAffinity? SessionAffinity { get; set; }
    public List<Destination>? Destinations { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}