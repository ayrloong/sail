using System.Collections.Immutable;
using Sail.Api.V1;
using Sail.Compass.Informers;
using EventType = Sail.Compass.Informers.EventType;

namespace Sail.Compass.Caching;

public class DefaultCache : ICache
{
    private readonly Lock _sync = new();
    private readonly Dictionary<string, Route> _routes = new();
    private readonly Dictionary<string, Cluster> _clusters = new();
    private readonly Dictionary<string, Certificate> _certificates = new();

    public void UpdateRoute(ResourceEvent<Route> resource)
    {
        var id = resource.Value.RouteId;
        lock (_sync)
        {
            if (resource.EventType == EventType.Deleted)
            {
                _routes.Remove(id);
                return;
            }

            _routes[id] = resource.Value;
        }
    }

    public void UpdateCluster(ResourceEvent<Cluster> resource)
    {
        var id = resource.Value.ClusterId;
        lock (_sync)
        {
            if (resource.EventType == EventType.Deleted)
            {
                _clusters.Remove(id);
                return;
            }

            _clusters[id] = resource.Value;
        }
    }

    public void UpdateCertificate(ResourceEvent<Certificate> resource)
    {
        var id = resource.Value.CertificateId;
        lock (_sync)
        {
            if (resource.EventType == EventType.Deleted)
            {
                _certificates.Remove(id);
                return;
            }

            _certificates[id] = resource.Value;
        }
    }

    public IReadOnlyList<Route> GetRoutes()
    {
        return _routes.Values.ToImmutableList();
    }

    public IReadOnlyList<Cluster> GetClusters()
    {
        return _clusters.Values.ToImmutableList();
    }

    public IReadOnlyList<Certificate> GetCertificates()
    {
        return _certificates.Values.ToImmutableList();
    }
}