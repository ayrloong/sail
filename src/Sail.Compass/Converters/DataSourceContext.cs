using Sail.Api.V1;

namespace Sail.Compass.Converters;

internal class DataSourceContext(IReadOnlyList<Cluster> clusters, IReadOnlyList<Route> routes)
{
    public IReadOnlyList<Cluster> Clusters { get; } = clusters;
    public IReadOnlyList<Route> Routes { get; } = routes;
}