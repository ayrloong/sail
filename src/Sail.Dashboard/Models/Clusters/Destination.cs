namespace Sail.Dashboard.Models.Clusters;

public class Destination
{
    public Guid Id { get; set; }
    public string? Address { get; set; }
    public string? Health { get; set; }
    public string? Host { get; set; }
}