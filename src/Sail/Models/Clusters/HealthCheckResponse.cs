namespace Sail.Models.Clusters;

public record HealthCheckResponse
{
    public string? AvailableDestinationsPolicy { get; set; }
    public ActiveHealthCheckResponse? Active { get; init; }
    public PassiveHealthCheckResponse? Passive { get; init; }
}