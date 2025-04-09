using ErrorOr;
using Sail.Core.Entities;
using MongoDB.Driver;
using Sail.Models.Clusters;
using Sail.Storage.MongoDB;

namespace Sail.Services;

public class ClusterService(SailContext context)
{
    public async Task<IEnumerable<ClusterResponse>> GetAsync(string keywords,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<Cluster>.Filter.Where(cluster => cluster.Name != null && cluster.Name.Contains(keywords));
        var routes = await context.Clusters.FindAsync(filter, cancellationToken: cancellationToken);
        var items = await routes.ToListAsync(cancellationToken: cancellationToken);
        return items.Select(MapToCluster);
    }

    public async Task<ErrorOr<Created>> CreateAsync(ClusterRequest request,
        CancellationToken cancellationToken = default)
    {
        var cluster = CreateClusterFromRequest(request);
        await context.Clusters.InsertOneAsync(cluster, cancellationToken: cancellationToken);
        return Result.Created;
    }

    public async Task<ErrorOr<Updated>> UpdateAsync(Guid id, ClusterRequest request,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<Cluster>.Filter.And(Builders<Cluster>.Filter.Where(x => x.Id == id));

        var update = Builders<Cluster>.Update
            .Set(x => x.Name, request.Name)
            .Set(x => x.ServiceName, request.ServiceName)
            .Set(x => x.ServiceDiscoveryType, request.ServiceDiscoveryType)
            .Set(x => x.LoadBalancingPolicy, request.LoadBalancingPolicy)
            .Set(x => x.Destinations, request.Destinations.Select(item => new Destination
            {
                Host = item.Host,
                Address = item.Address,
                Health = item.Health
            }).ToList())
            .Set(x => x.HealthCheck, new HealthCheck
            {
                AvailableDestinationsPolicy = request.HealthCheck?.AvailableDestinationsPolicy,
                Active = new ActiveHealthCheck
                {
                    Enabled = request.HealthCheck?.Active?.Enabled,
                    Interval = request.HealthCheck?.Active?.Interval,
                    Timeout = request.HealthCheck?.Active?.Timeout,
                    Policy = request.HealthCheck?.Active?.Policy,
                    Path = request.HealthCheck?.Active?.Path,
                    Query = request.HealthCheck?.Active?.Query
                },
                Passive = new PassiveHealthCheck
                {
                    Enabled = request.HealthCheck?.Passive?.Enabled,
                    Policy = request.HealthCheck?.Passive?.Policy,
                    ReactivationPeriod = request.HealthCheck?.Passive?.ReactivationPeriod,
                }
            })
            .Set(x => x.UpdatedAt, DateTimeOffset.UtcNow);

        await context.Clusters.FindOneAndUpdateAsync(filter, update, cancellationToken: cancellationToken);
        return Result.Updated;
    }

    public async Task<ErrorOr<Deleted>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Cluster>.Filter.And(Builders<Cluster>.Filter.Where(x => x.Id == id));
        await context.Clusters.DeleteOneAsync(filter, cancellationToken);
        return Result.Deleted;
    }

    private Cluster CreateClusterFromRequest(ClusterRequest request)
    {
        var cluster = new Cluster
        {
            Name = request.Name,
            LoadBalancingPolicy = request.LoadBalancingPolicy,
            ServiceName = request.ServiceName,
            ServiceDiscoveryType = request.ServiceDiscoveryType,
            HealthCheck = new HealthCheck
            {
                AvailableDestinationsPolicy = request.HealthCheck?.AvailableDestinationsPolicy,
                Active = new ActiveHealthCheck
                {
                    Enabled = request.HealthCheck?.Active?.Enabled,
                    Interval = request.HealthCheck?.Active?.Interval,
                    Timeout = request.HealthCheck?.Active?.Timeout,
                    Policy = request.HealthCheck?.Active?.Policy,
                    Path = request.HealthCheck?.Active?.Path,
                    Query = request.HealthCheck?.Active?.Query
                },
                Passive = new PassiveHealthCheck
                {
                    Enabled = request.HealthCheck?.Passive?.Enabled,
                    Policy = request.HealthCheck?.Passive?.Policy,
                    ReactivationPeriod = request.HealthCheck?.Passive?.ReactivationPeriod,
                }
            },
            Destinations = request.Destinations.Select(item => new Destination
            {
                Host = item.Host,
                Address = item.Address,
                Health = item.Health
            }).ToList()
        };
        return cluster;
    }

    private ClusterResponse MapToCluster(Cluster cluster)
    {
        return new ClusterResponse
        {
            Id = cluster.Id,
            Name = cluster.Name,
            ServiceName = cluster.ServiceName,
            ServiceDiscoveryType = cluster.ServiceDiscoveryType,
            LoadBalancingPolicy = cluster.LoadBalancingPolicy,
            HealthCheck = new HealthCheckResponse
            {
                AvailableDestinationsPolicy = cluster.HealthCheck?.AvailableDestinationsPolicy,
                Active = new ActiveHealthCheckResponse
                {
                    Enabled = cluster.HealthCheck?.Active?.Enabled,
                    Interval = cluster.HealthCheck?.Active?.Interval,
                    Timeout = cluster.HealthCheck?.Active?.Timeout,
                    Policy = cluster.HealthCheck?.Active?.Policy,
                    Path = cluster.HealthCheck?.Active?.Path,
                    Query = cluster.HealthCheck?.Active?.Query
                },
                Passive = new PassiveHealthCheckResponse
                {
                    Enabled = cluster.HealthCheck?.Passive?.Enabled,
                    Policy = cluster.HealthCheck?.Passive?.Policy,
                    ReactivationPeriod = cluster.HealthCheck?.Passive?.ReactivationPeriod,
                }
            },
            Destinations = cluster.Destinations?.Select(d => new DestinationResponse
            {
                Host = d.Host,
                Address = d.Address,
                Health = d.Health
            }),
            CreatedAt = cluster.CreatedAt,
            UpdatedAt = cluster.UpdatedAt
        };
    }
}