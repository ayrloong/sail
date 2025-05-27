namespace Sail.Models.Clusters;

public record PassiveHealthCheckResponse
{
    public bool? Enabled { get; init; }
    public string? Policy { get; init; }
    public TimeSpan? ReactivationPeriod { get; init; }
}