﻿using k8s.Models;

namespace Sail.Kubernetes.Gateway.Caching;

public struct Endpoints
{
    public Endpoints(V1Endpoints endpoints)
    {
        if (endpoints is null)
        {
            throw new ArgumentNullException(nameof(endpoints));
        }

        Name = endpoints.Name();
        Subsets = endpoints.Subsets;
    }

    public string Name { get; set; }
    public IList<V1EndpointSubset> Subsets { get; }
}