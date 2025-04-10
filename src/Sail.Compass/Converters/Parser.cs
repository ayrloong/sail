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

    public void ConvertFromDataSource(DataSourceContext dataSourceContext,
        YarpConfigContext configContext, CancellationToken cancellationToken)
    {
        foreach (var route in dataSourceContext.Routes)
        {
            HandleRoute(configContext, route);
        }

        foreach (var cluster in dataSourceContext.Clusters)
        {
            HandleCluster(configContext, cluster);
        }
    }

    private void HandleCluster(YarpConfigContext configContext, Cluster cluster)
    {
        var clusters = configContext.Clusters;
        var clusterConfig = new ClusterConfig
        {
            ClusterId = cluster.ClusterId,
            LoadBalancingPolicy = cluster.LoadBalancingPolicy,
            HealthCheck = new HealthCheckConfig
            {
                AvailableDestinationsPolicy = cluster.HealthCheck?.AvailableDestinationsPolicy,
                Active = new ActiveHealthCheckConfig
                {
                    Enabled = cluster.HealthCheck?.Active?.Enabled,
                    Interval = TimeSpan.TryParse(cluster.HealthCheck?.Active?.Interval, CultureInfo.InvariantCulture,
                        out var interval)
                        ? interval
                        : null,
                    Timeout = TimeSpan.TryParse(cluster.HealthCheck?.Active?.Timeout, CultureInfo.InvariantCulture,
                        out var timeout)
                        ? timeout
                        : null,
                    Policy = cluster.HealthCheck?.Active?.Policy,
                    Path = cluster.HealthCheck?.Active?.Path,
                    Query = cluster.HealthCheck?.Active?.Query
                },
                Passive = new PassiveHealthCheckConfig
                {
                    Enabled = cluster.HealthCheck?.Passive?.Enabled,
                    Policy = cluster.HealthCheck?.Passive?.Policy,
                    ReactivationPeriod = TimeSpan.TryParse(cluster.HealthCheck?.Passive?.ReactivationPeriod,
                        CultureInfo.InvariantCulture,
                        out var reactivationPeriod)
                        ? reactivationPeriod
                        : null
                }
            },
            SessionAffinity = new SessionAffinityConfig
            {

                Enabled = cluster.SessionAffinity?.Enabled,
                Policy = cluster.SessionAffinity?.Policy,
                FailurePolicy = cluster.SessionAffinity?.FailurePolicy,
                AffinityKeyName = cluster.SessionAffinity?.AffinityKeyName ?? string.Empty,
                Cookie = new SessionAffinityCookieConfig
                {
                    Path = cluster.SessionAffinity?.Cookie?.Path,
                    Domain = cluster.SessionAffinity?.Cookie?.Domain,
                    HttpOnly = cluster.SessionAffinity?.Cookie?.HttpOnly,
                    Expiration = TimeSpan.TryParse(cluster.SessionAffinity?.Cookie?.Expiration,
                        CultureInfo.InvariantCulture,
                        out var expiration)
                        ? expiration
                        : null,
                    MaxAge = TimeSpan.TryParse(cluster.SessionAffinity?.Cookie?.MaxAge, CultureInfo.InvariantCulture,
                        out var maxAge)
                        ? maxAge
                        : null,
                    IsEssential = cluster.SessionAffinity?.Cookie?.IsEssential
                }
            },
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
    }

    private void HandleRoute(YarpConfigContext configContext, Route route)
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
            Transforms = route.Transforms.Select(x => x.Properties).ToList(),
            Order = route.Order
        };
        routes.Add(routeConfig);
    }
}