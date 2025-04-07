namespace Sail.Models.Route;

public record RouteMatchVm
{
    public List<string> Methods { get; init; }
    public List<string> Hosts { get; init; }
    public IEnumerable<RouteHeaderVm> Headers  { get; init; }
    public string Path { get; init; }
    public IEnumerable<RouteQueryParameterVm> QueryParameters { get; init; }
}
