using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MongoDB.Driver;
using Sail.Api.V1;
using Sail.Core.Entities;
using Sail.Core.Stores;
using Sail.Storage.MongoDB;
using Sail.Storage.MongoDB.Extensions;
using Cluster = Sail.Core.Entities.Cluster;
using ClusterResponse =  Sail.Api.V1.Cluster;
using Destination = Sail.Api.V1.Destination;

namespace Sail.Grpc;

public class ClusterGrpcService(SailContext dbContext, IClusterStore clusterStore) : ClusterService.ClusterServiceBase
{
    public override async Task<ListClusterResponse> List(Empty request, ServerCallContext context)
    {
        var clusters = await clusterStore.GetAsync(CancellationToken.None);
        var response = MapToClusterItemsResponse(clusters);
        return response;

    }

    public override async Task Watch(Empty request, IServerStreamWriter<WatchClusterResponse> responseStream,
        ServerCallContext context)
    {
        var options = new ChangeStreamOptions
        {
            FullDocument = ChangeStreamFullDocumentOption.Required,
            FullDocumentBeforeChange = ChangeStreamFullDocumentBeforeChangeOption.Required
        };

        while (!context.CancellationToken.IsCancellationRequested)
        {
            var watch = await dbContext.Clusters.WatchAsync(options);

            await foreach (var changeStreamDocument in watch.ToAsyncEnumerable())
            {
                var document = changeStreamDocument.FullDocument;
                if (changeStreamDocument.OperationType == ChangeStreamOperationType.Delete)
                {
                    document = changeStreamDocument.FullDocumentBeforeChange;
                }

                var eventType = changeStreamDocument.OperationType switch
                {
                    ChangeStreamOperationType.Create => EventType.Create,
                    ChangeStreamOperationType.Update => EventType.Update,
                    ChangeStreamOperationType.Delete => EventType.Delete,
                    _ => EventType.Unknown
                };
                var cluster = MapToClusterResponse(document);
                var response = new WatchClusterResponse
                {
                    Cluster = cluster,
                    EventType = eventType
                };
                await responseStream.WriteAsync(response);
            }
        }
    }

    private static ListClusterResponse MapToClusterItemsResponse(List<Cluster> clusters)
    {
        var items = clusters.Select(MapToClusterResponse);

        var response = new ListClusterResponse
        {
            Items = { items }
        };
        return response;
    }

    private static ClusterResponse MapToClusterResponse(Cluster cluster)
    {
        return new ClusterResponse
        {
            ClusterId = cluster.Id.ToString(),
            ServiceName = cluster.ServiceName,
            EnabledServiceDiscovery = IsServiceDiscoveryEnabled(cluster.ServiceName, cluster.ServiceDiscoveryType),
            LoadBalancingPolicy = cluster.LoadBalancingPolicy,
            HealthCheck = new ClusterHealthCheck
            {
                AvailableDestinationsPolicy = cluster.HealthCheck?.AvailableDestinationsPolicy,
                Active = new ClusterHealthCheck.Types.ActiveHealthCheck
                {
                    Enabled = cluster.HealthCheck?.Active?.Enabled ?? false,
                    Interval = cluster.HealthCheck?.Active?.Interval?.ToString(),
                    Timeout = cluster.HealthCheck?.Active?.Timeout?.ToString(),
                    Policy = cluster.HealthCheck?.Active?.Policy,
                    Path = cluster.HealthCheck?.Active?.Path,
                    Query = cluster.HealthCheck?.Active?.Query
                },
                Passive = new ClusterHealthCheck.Types.PassiveHealthCheck
                {
                    Enabled = cluster.HealthCheck?.Passive?.Enabled ?? false,
                    Policy = cluster.HealthCheck?.Passive?.Policy,
                    ReactivationPeriod = cluster.HealthCheck?.Passive?.ReactivationPeriod?.ToString()
                }
            },
            Destinations =
            {
                cluster.Destinations?.Select(d => new Destination
                {
                    Address = d.Address,
                    Health = d.Health,
                    Host = d.Host
                })
            }
        };
    }

    private static bool IsServiceDiscoveryEnabled(string? serviceName, ServiceDiscoveryType? serviceDiscoveryType)
    {
        return !string.IsNullOrEmpty(serviceName) && serviceDiscoveryType is not null;
    }
}