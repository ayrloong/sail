using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sail.Api.V1;
using Sail.Compass.Caching;
using Sail.Compass.Converters;
using Sail.Compass.Hosting;
using Sail.Compass.Informers;
using Sail.Compass.Services;
using Sail.Core.Certificates;
using Sail.Core.ConfigProvider;
using Yarp.ReverseProxy.Configuration;

namespace Sail.Compass.Management;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddControllerRuntime(this IServiceCollection services)
    {
        services.AddSingleton<Parser>();
        services.AddSingleton<ICache, DefaultCache>();
        services.AddTransient<IReconciler, Reconciler>();
        services.AddHostedService<ProxyDiscoveryService>();
        services.RegisterResourceInformer<Route, V1RouteResourceInformer>();
        services.RegisterResourceInformer<Cluster, V1ClusterResourceInformer>();
        services.RegisterResourceInformer<Certificate, V1CertificateResourceInformer>();
        
        return services;
    }
    private static IServiceCollection RegisterResourceInformer<TResource, TService>(this IServiceCollection services)
        where TResource : class
        where TService : IResourceInformer<TResource>
    {
        services.AddSingleton(typeof(IResourceInformer<TResource>), typeof(TService)); 

        return services.RegisterHostedService<IResourceInformer<TResource>>();
    }
    public static IReverseProxyBuilder LoadFromMessages(this IReverseProxyBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var configProvider = new ProxyConfigProvider();
        builder.Services.AddSingleton<IProxyConfigProvider>(configProvider);
        builder.Services.AddSingleton<IUpdateConfig>(configProvider);
        return builder;
    }
}