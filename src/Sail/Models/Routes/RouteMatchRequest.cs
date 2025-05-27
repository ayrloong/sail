namespace Sail.Models.Routes;

public record RouteMatchRequest(
    string Path,
    List<string>? Methods,
    List<string>? Hosts,
    List<QueryParameterRequest>? QueryParameters,
    List<RouteHeaderRequest>? Headers);