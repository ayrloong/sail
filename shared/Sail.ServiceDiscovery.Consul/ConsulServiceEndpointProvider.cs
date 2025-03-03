using System.Net;
using Consul;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.ServiceDiscovery;

namespace Sail.ServiceDiscovery.Consul;

internal sealed  class ConsulServiceEndpointProvider(
    ServiceEndpointQuery query,
    string hostName,
    IOptionsMonitor<ConsulServiceEndpointProviderOptions> options,
    ILogger<ConsulServiceEndpointProvider> logger,
    IConsulClient client,
    TimeProvider timeProvider) :
    ConsulServiceEndpointProviderBase(query, logger, timeProvider), IHostNameFeature
{
    protected override double RetryBackOffFactor { get; } = options.CurrentValue.RetryBackOffFactor;
    protected override TimeSpan MinRetryPeriod { get; } = options.CurrentValue.MinRetryPeriod;
    protected override TimeSpan MaxRetryPeriod { get; } = options.CurrentValue.MaxRetryPeriod;
    protected override TimeSpan DefaultRefreshPeriod { get; } = options.CurrentValue.DefaultRefreshPeriod;

    public string HostName { get; } = hostName;

    protected override async Task ResolveAsyncCore()
    {
        var endpoints = new List<ServiceEndpoint>();
        var ttl = DefaultRefreshPeriod;
        Log.AddressQuery(logger, ServiceName, hostName);
        var result = await client.Health.Service(hostName).ConfigureAwait(false);
        foreach (var service in result.Response)
        {
            var ipAddress = new IPAddress(service.Service.Address.Split('.').Select(a => Convert.ToByte(a)).ToArray());
            var ipPoint = new IPEndPoint(ipAddress, service.Service.Port);
            var serviceEndpoint = ServiceEndpoint.Create(ipPoint);
            serviceEndpoint.Features.Set<IServiceEndpointProvider>(this);
            endpoints.Add(serviceEndpoint);
        }

        if (endpoints.Count == 0)
        {
            throw new InvalidOperationException(
                $"No records were found for service '{ServiceName}' ( name: '{hostName}').");
        }

        SetResult(endpoints, ttl);
    }
}