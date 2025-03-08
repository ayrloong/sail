using Microsoft.Extensions.DependencyInjection;

namespace Yarp.Extensions.Resilience.ServiceDiscovery;

public static class ServiceDiscoveryServiceCollectionExtensions
{
    public static IReverseProxyBuilder AddServiceDiscoveryDestinationResolver(this IReverseProxyBuilder builder,
        Action<ServiceDiscoveryOptions>? configureOptions = null)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.AddServiceDiscoveryCore();
        builder.Services.AddSingleton<IServiceDiscoveryDestinationResolver, ServiceDiscoveryDestinationResolver>();

        if (configureOptions is not null)
        {
            builder.Services.Configure(configureOptions);
        }

        return builder;
    }
}