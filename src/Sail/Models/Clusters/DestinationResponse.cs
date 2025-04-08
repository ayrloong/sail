namespace Sail.Models.Clusters;

public record DestinationResponse
{
    public string? Address { get; init; }
    public string? Health { get; init; }
    public string? Host { get; init; }
}