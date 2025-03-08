namespace Yarp.Extensions.Resilience.ServiceDiscovery;

public class ServiceDiscoveryOptions
{
    public TimeSpan? RefreshPeriod { get; set; } = TimeSpan.FromMinutes(5);
}