namespace Sail.Models.Route;

public record RouteVm
{
    public Guid Id { get; init; }
    public Guid? ClusterId { get; init; }
    public string Name { get; init; }
    public RouteMatchVm Match { get; init; }
    public int Order { get; init; }
    public string? AuthorizationPolicy { get; init; }
    public string? RateLimiterPolicy { get; init; }
    public string? CorsPolicy { get; init; }
    public string? TimeoutPolicy { get; init; }
    public TimeSpan? Timeout { get; init; }
    public long? MaxRequestBodySize { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
}