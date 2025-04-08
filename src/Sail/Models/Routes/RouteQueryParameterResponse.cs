using Sail.Core.Entities;

namespace Sail.Models.Routes;

public class RouteQueryParameterResponse
{
    public string Name { get; init; }
    public List<string> Values { get; init; }
    public QueryParameterMatchMode Mode { get; init; }
    public bool IsCaseSensitive { get; init; }
}