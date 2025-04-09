using Sail.Core.Entities;

namespace Sail.Models.Clusters;

public record ClusterRequest(
    string Name,
    string ServiceName,
    ServiceDiscoveryType? ServiceDiscoveryType,
    HealthCheckRequest? HealthCheck,
    string LoadBalancingPolicy,
    List<DestinationRequest> Destinations);