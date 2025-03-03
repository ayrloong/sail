using Sail.Api.V1;
using Sail.Core.Certificates;
using Yarp.Extensions.Resilience.ServiceDiscovery;
using Yarp.ReverseProxy.Configuration;
using RouteMatch = Yarp.ReverseProxy.Configuration.RouteMatch;

namespace Sail.Compass.Converters;

internal class Parser(IServiceDiscoveryDestinationResolver resolver)
{
    IReadOnlyList<CertificateConfig> ConvertCertificates(IEnumerable<Certificate> certificates)
    {
        throw new NotImplementedException();
    }

    public async ValueTask ConvertFromDataSourceAsync(DataSourceContext dataSourceContext,
        YarpConfigContext configContext, CancellationToken cancellationToken)
    {
        foreach (var route in dataSourceContext.Routes)
        {
            await HandleRouteAsyncCore(configContext, route, cancellationToken);
        }

        foreach (var cluster in dataSourceContext.Clusters)
        {
            await HandleClusterAsyncCore(configContext, cluster, cancellationToken);
        }
    }

    async ValueTask HandleClusterAsyncCore(YarpConfigContext configContext, Cluster cluster,
        CancellationToken cancellationToken)
    {
        var clusters = configContext.Clusters;

        var clusterConfig = new ClusterConfig
        {
            ClusterId = cluster.ClusterId,
            LoadBalancingPolicy = cluster.LoadBalancingPolicy,
            Destinations = cluster.Destinations?.ToDictionary(x => x.DestinationId, x => new DestinationConfig
            {
                Host = x.Host,
                Health = x.Health,
                Address = x.Address
            })
        };

        if (cluster.EnabledServiceDiscovery)
        {
            var resolvedDestinations = await resolver.ResolveDestinationsAsync(cluster.ServiceName, cancellationToken);
            clusterConfig = clusterConfig with { Destinations = resolvedDestinations.Destinations };
        }

        clusters.Add(clusterConfig);
    }

    ValueTask HandleRouteAsyncCore(YarpConfigContext configContext, Route route, CancellationToken cancellationToken)
    {
        var routes = configContext.Routes;

        var routeConfig = new RouteConfig
        {
            RouteId = route.RouteId,
            ClusterId = route.ClusterId,
            Match = new RouteMatch
            {
                Hosts = route.Match.Hosts,
                Path = route.Match.Path,
                Methods = route.Match.Methods,
                Headers = route.Match.Headers.Select(x => new RouteHeader
                {
                    Name = x.Name,
                    Values = x.Values,
                    Mode = (HeaderMatchMode)x.Mode,
                    IsCaseSensitive = x.IsCaseSensitive
                }).ToList(),
                QueryParameters = route.Match.QueryParameters.Select(x => new RouteQueryParameter
                {
                    Name = x.Name,
                    Values = x.Values,
                    Mode = (QueryParameterMatchMode)x.Mode,
                    IsCaseSensitive = x.IsCaseSensitive
                }).ToList()
            },
            AuthorizationPolicy = route.AuthorizationPolicy,
            RateLimiterPolicy = route.RateLimiterPolicy,
            TimeoutPolicy = route.TimeoutPolicy,
            CorsPolicy = route.CorsPolicy,
         //   Timeout = TimeSpan.FromSeconds(route.Timeout),
            MaxRequestBodySize = route.MaxRequestBodySize,
            Order = route.Order
        };

        routes.Add(routeConfig);
        return ValueTask.CompletedTask;
    }
}