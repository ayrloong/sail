namespace Sail.Models.Clusters;

public record ClusterResponse
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? ServiceName { get; init; }
    public string? ServiceDiscoveryType { get; init; }
    public string? LoadBalancingPolicy { get; init; }
    public IEnumerable<DestinationResponse>? Destinations { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
}