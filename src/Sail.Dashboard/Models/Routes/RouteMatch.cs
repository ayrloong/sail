namespace Sail.Dashboard.Models.Routes;

public class RouteMatch
{
    public List<string>? Methods { get; set; }
    public List<string>? Hosts { get; set; }
    public List<RouteHeader>? Headers { get; set; }
    public string Path { get; set; }
    public List<RouteQueryParameter>? QueryParameters { get; set; }
}