using Sail.Compass.Caching;
using Sail.Compass.Converters;
using Sail.Core.ConfigProvider;

namespace Sail.Compass.Services;

internal class Reconciler(ICache cache, Parser parser, IUpdateConfig updateConfig) : IReconciler
{
    public async Task ProcessProxyAsync(CancellationToken cancellationToken)
    {
        var configContext = new YarpConfigContext();
        var clusters = cache.GetClusters();
        var routes = cache.GetRoutes();
        var context = new DataSourceContext(clusters, routes);

        parser.ConvertFromDataSource(context, configContext, cancellationToken);
        await updateConfig.UpdateAsync(configContext.Routes, configContext.Clusters,
            cancellationToken);
    }
    
    public Task ProcessCertificateAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}