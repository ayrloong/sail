using Sail.Core.Entities;

namespace Sail.Models.Clusters;

public record ClusterRequest(
    string Name,
    string ServiceName,
    ServiceDiscoveryType? ServiceDiscoveryType,
    HealthCheckRequest? HealthCheck,
    SessionAffinityRequest? SessionAffinity,
    string LoadBalancingPolicy,
    List<DestinationRequest> Destinations);