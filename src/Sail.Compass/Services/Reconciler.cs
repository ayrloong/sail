using Sail.Api.V1;
using Sail.Compass.Caching;
using Sail.Compass.Converters;
using Sail.Core.ConfigProvider;

namespace Sail.Compass.Services;

internal class Reconciler(ICache cache, Parser parser, IUpdateConfig updateConfig) : IReconciler
{
    public async Task ProcessAsync(CancellationToken cancellationToken)
    {
        var configContext = new YarpConfigContext();
        var clusters = cache.GetClusters();
        var routes = cache.GetRoutes();
        var context = new DataSourceContext(clusters, routes);

        await parser.ConvertFromDataSourceAsync(context, configContext, cancellationToken);
        
        await updateConfig.UpdateAsync(configContext.Routes, configContext.Clusters,
            cancellationToken);
    }
}