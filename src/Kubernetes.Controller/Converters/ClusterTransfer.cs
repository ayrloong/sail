﻿using Yarp.ReverseProxy.Configuration;

namespace Sail.Kubernetes.Controller.Converters;

public class ClusterTransfer
{
    public Dictionary<string, DestinationConfig> Destinations { get; set; } =
        new Dictionary<string, DestinationConfig>();

    public string ClusterId { get; set; }
    public string LoadBalancingPolicy { get; set; }
    public SessionAffinityConfig SessionAffinity { get; set; }
    public HealthCheckConfig HealthCheck { get; set; }
    public HttpClientConfig HttpClientConfig { get; set; }
    public CanaryConfig Canary { get; set; }
}