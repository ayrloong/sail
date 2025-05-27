namespace Sail.Dashboard.Models.Clusters;

public class ClusterRequest
{
    public string? Name { get; set; }
    public string? ServiceName { get; set; }
    public string? LoadBalancingPolicy { get; set; }
    public HealthCheck? HealthCheck { get; set; }
    public SessionAffinity? SessionAffinity { get; set; }
    public List<Destination>? Destinations { get; set; }
}