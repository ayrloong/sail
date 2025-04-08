namespace Sail.Models.Clusters;

public record ClusterRequest(
    string Name,
    string ServiceName,
    string ServiceDiscoveryType,
    string LoadBalancingPolicy,
    List<DestinationRequest> Destinations);