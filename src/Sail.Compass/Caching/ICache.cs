using Sail.Api.V1;
using Sail.Compass.Informers;

namespace Sail.Compass.Caching;

public interface ICache
{
    void UpdateRoute(ResourceEvent<Route> resource);
    void UpdateCluster(ResourceEvent<Cluster> resource);
    void UpdateCertificate(ResourceEvent<Certificate> resource);
    IReadOnlyList<Route> GetRoutes();
    IReadOnlyList<Cluster> GetClusters();
    IReadOnlyList<Certificate> GetCertificates();
}