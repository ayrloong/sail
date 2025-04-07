namespace Sail.Models.Route;

public record RouteRequest(Guid ClusterId, string Name, RouteMatchRequest Match);
