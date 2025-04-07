using System.Globalization;
using Sail.Api.V1;
using Sail.Core.Certificates;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Health;
using RouteMatch = Yarp.ReverseProxy.Configuration.RouteMatch;

namespace Sail.Compass.Converters;

internal class Parser(IClusterDestinationsUpdater destinationsUpdater)
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

    ValueTask HandleClusterAsyncCore(YarpConfigContext configContext, Cluster cluster,
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
            clusterConfig = clusterConfig with
            {
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    {
                        cluster.ServiceName, new DestinationConfig
                        {
                            Address = $"http://{cluster.ServiceName}",
                            Host = cluster.ServiceName
                        }
                    }
                }
            };
        }

        clusters.Add(clusterConfig);
        return ValueTask.CompletedTask;
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
            Timeout = TimeSpan.TryParse(route.Timeout, CultureInfo.InvariantCulture, out var timeout) ? timeout : null,
            TimeoutPolicy = route.TimeoutPolicy,
            CorsPolicy = route.CorsPolicy,
            MaxRequestBodySize = route.MaxRequestBodySize,
            Order = route.Order
        };
        routes.Add(routeConfig);
        return ValueTask.CompletedTask;
    }
}