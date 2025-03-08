using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.ServiceDiscovery;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.ServiceDiscovery;

namespace Yarp.Extensions.Resilience.ServiceDiscovery;

public class ServiceDiscoveryDestinationResolver(
    ServiceEndpointResolver resolver,
    IOptions<ServiceDiscoveryOptions> options)
    : IServiceDiscoveryDestinationResolver
{


    public async ValueTask<ResolvedDestinationCollection> ResolveDestinationsAsync(string serviceName,
        CancellationToken cancellationToken)
    {
        var result = await resolver.GetEndpointsAsync(serviceName, cancellationToken).ConfigureAwait(false);

        var destinations = new Dictionary<string, DestinationConfig>();

        foreach (var endpoint in result.Endpoints)
        {
            var addressString = endpoint.ToString()!;

            var config = new DestinationConfig
            {
                Address = $"{addressString}",
            };

            var name = $"{serviceName}[{addressString}]";
            destinations.Add(name, config);
        }

        var changeToken = options.Value.RefreshPeriod switch
        {
            { } refreshPeriod when refreshPeriod > TimeSpan.Zero => new CancellationChangeToken(
                new CancellationTokenSource(refreshPeriod).Token),
            _ => null,
        };

        return new ResolvedDestinationCollection(destinations, changeToken);
    }
}