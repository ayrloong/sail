using Microsoft.Extensions.DependencyInjection;

namespace Yarp.Extensions.Resilience.ServiceDiscovery;

public static class ServiceDiscoveryServiceCollectionExtensions
{
    public static IReverseProxyBuilder AddServiceDiscoveryDestinationResolver(this IReverseProxyBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.AddServiceDiscoveryCore();
        builder.Services.AddSingleton<IServiceDiscoveryDestinationResolver, ServiceDiscoveryDestinationResolver>();
        return builder;
    }
}