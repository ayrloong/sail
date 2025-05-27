namespace Sail.Dashboard.Models.Routes;

public class RouteQueryParameter
{
    public string Name { get; set; }
    public List<string>? Values { get; set; }
    public QueryParameterMatchMode Mode { get; set; }
    public bool IsCaseSensitive { get; set; }
}