namespace Sail.Dashboard.Models.Clusters;

public class HealthCheck
{
    public string? AvailableDestinationsPolicy { get; set; }
    public ActiveHealthCheck? Active { get; set; }
    public PassiveHealthCheck? Passive { get; set; }
}