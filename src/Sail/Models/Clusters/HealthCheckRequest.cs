namespace Sail.Models.Clusters;

public record HealthCheckRequest
{
    public string? AvailableDestinationsPolicy { get; set; }
    public ActiveHealthCheckRequest? Active { get; init; }
    public PassiveHealthCheckRequest? Passive { get; init; }
}