namespace Sail.Models.Routes;

public record RouteRequest(Guid ClusterId, string Name, RouteMatchRequest Match);
