using ErrorOr;
using Sail.Core.Entities;
using MongoDB.Driver;
using Sail.Models.Clusters;
using Sail.Storage.MongoDB;

namespace Sail.Services;

public class ClusterService(SailContext context)
{
    public async Task<IEnumerable<ClusterResponse>> GetAsync(CancellationToken cancellationToken = default)
    {
        var filter = Builders<Cluster>.Filter.Empty;
        var routes = await context.Clusters.FindAsync(filter, cancellationToken: cancellationToken);
        var items = await routes.ToListAsync(cancellationToken: cancellationToken);

        return items.Select(MapToCluster);
    }

    public async Task<ErrorOr<Created>> CreateAsync(ClusterRequest request,CancellationToken cancellationToken = default)
    {
        var cluster = new Cluster
        {
            Name = request.Name,
            LoadBalancingPolicy = request.LoadBalancingPolicy,
            ServiceName = request.ServiceName,
            ServiceDiscoveryType = request.ServiceDiscoveryType,
            Destinations = request.Destinations.Select(item => new Destination
            {
                Host = item.Host,
                Address = item.Address,
                Health = item.Health
            }).ToList()
        };
        await context.Clusters.InsertOneAsync(cluster, cancellationToken: cancellationToken);
        return Result.Created;
    }

    public async Task<ErrorOr<Updated>> UpdateAsync(Guid id, ClusterRequest request,CancellationToken cancellationToken = default)
    {
        var filter = Builders<Cluster>.Filter.And(Builders<Cluster>.Filter.Where(x => x.Id == id));
        
        var update = Builders<Cluster>.Update
            .Set(x => x.Name,request.Name)
            .Set(x => x.LoadBalancingPolicy, request.LoadBalancingPolicy)
            .Set(x => x.UpdatedAt, DateTimeOffset.UtcNow);

        await context.Clusters.FindOneAndUpdateAsync(filter, update, cancellationToken: cancellationToken);
        return Result.Updated;
    }

    public async Task<ErrorOr<Deleted>> DeleteAsync(Guid id,CancellationToken cancellationToken = default)
    {
        var filter = Builders<Cluster>.Filter.And(Builders<Cluster>.Filter.Where(x => x.Id == id));
        await context.Clusters.DeleteOneAsync(filter, cancellationToken);
        return Result.Deleted;
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