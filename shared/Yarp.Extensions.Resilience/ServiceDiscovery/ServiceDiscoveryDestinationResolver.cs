using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.ServiceDiscovery;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.ServiceDiscovery;

namespace Yarp.Extensions.Resilience.ServiceDiscovery;

public class ServiceDiscoveryDestinationResolver(ServiceEndpointResolver resolver)
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

        return new ResolvedDestinationCollection(destinations, new CompositeChangeToken(new List<IChangeToken>()));
    }
    
    
}