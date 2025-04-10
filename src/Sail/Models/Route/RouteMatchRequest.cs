namespace Sail.Models.Route;

public record RouteMatchRequest(
    string Path,
    List<string>? Methods,
    List<string>? Hosts,
    List<QueryParameterRequest>? ParameterRequests,
    List<RouteHeaderRequest>? RouteHeaders);