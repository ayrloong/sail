﻿using System.Collections.Immutable;
using k8s;
using k8s.Models;
using Microsoft.Extensions.Options;
using Microsoft.Kubernetes;
using Sail.Kubernetes.Gateway.Models;
using Sail.Kubernetes.Gateway.Services;

namespace Sail.Kubernetes.Gateway.Caching;

public class GatewayCache : ICache
{
    private readonly object _sync = new object();

    private readonly Dictionary<string, GatewayClassData> _gatewayClassData = new();
    private readonly Dictionary<string, NamespaceCache> _namespaceCaches = new();
    private readonly SailOptions _options;
    private readonly ILogger<GatewayCache> _logger;

    private bool _isDefaultController;

    public GatewayCache(IOptions<SailOptions> options, ILogger<GatewayCache> logger)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void Update(WatchEventType eventType, V1beta1GatewayClass gatewayClass)
    {
        if (gatewayClass is null)
        {
            throw new ArgumentNullException(nameof(gatewayClass));
        }

        if (!string.Equals(_options.ControllerClass, gatewayClass.Spec.ControllerName,
                StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogInformation(
                "Ignoring {GatewayClassNamespace}/{GatewayClassName} as the spec.controller is not the same as this gateway",
                gatewayClass.Metadata.NamespaceProperty,
                gatewayClass.Metadata.Name);
            return;
        }

        var gatewayClassName = gatewayClass.Name();
        lock (_sync)
        {
            switch (eventType)
            {
                case WatchEventType.Added or WatchEventType.Modified:
                    _gatewayClassData[gatewayClassName] = new GatewayClassData(gatewayClass);
                    break;
                case WatchEventType.Deleted:
                    _gatewayClassData.Remove(gatewayClassName);
                    break;
            }

            _isDefaultController = _gatewayClassData.Values.Any(ic => ic.IsDefault);
        }
    }

    public bool Update(WatchEventType eventType, V1beta1Gateway gateway)
    {
        if (gateway is null)
        {
            throw new ArgumentNullException(nameof(gateway));
        }

        if (IsGateway(gateway.Spec))
        {
            Namespace(gateway.Namespace()).Update(eventType, gateway);
            return true;
        }

        if (eventType == WatchEventType.Modified && Namespace(gateway.Namespace()).GatewayExists(gateway))
        {
            _logger.LogInformation("Removing gateway {GatewayNamespace}/{GatewayName} because of unknown gateway class",
                gateway.Metadata.NamespaceProperty, gateway.Metadata.Name);
            Namespace(gateway.Namespace()).Update(WatchEventType.Deleted, gateway);
            return true;
        }

        _logger.LogInformation("Ignoring gateway {GatewayNamespace}/{GatewayName} because of gateway class",
            gateway.Metadata.NamespaceProperty, gateway.Metadata.Name);
        return false;
    }


    public bool Update(WatchEventType eventType, V1beta1HttpRoute httpRoute)
    {
        return true;
    }
    public ImmutableList<string> Update(WatchEventType eventType, V1Service service)
    {
        if (service is null)
        {
            throw new ArgumentNullException(nameof(service));
        }

        return Namespace(service.Namespace()).Update(eventType, service);
    }

    public ImmutableList<string> Update(WatchEventType eventType, V1Endpoints endpoints)
    {
        return Namespace(endpoints.Namespace()).Update(eventType, endpoints);
    }

    public bool TryGetReconcileData(NamespacedName key, out ReconcileData data)
    {
        return Namespace(key.Namespace).TryLookup(key, out data);
    }

    public void GetKeys(List<NamespacedName> keys)
    {
        lock (_sync)
        {
            foreach (var (ns, cache) in _namespaceCaches)
            {
                cache.GetKeys(ns, keys);
            }
        }
    }

    public IEnumerable<GatewayData> GetGatewayes()
    {
        var gatewayes = new List<GatewayData>();
        lock (_sync)
        {
            foreach (var ns in _namespaceCaches)
            {
                gatewayes.AddRange(ns.Value.GetGatewayes());
            }
        }

        return gatewayes;
    }

    private bool IsGateway(V1beta1GatewaySpec spec)
    {
        if (spec.GatewayClassName is not null)
        {
            lock (_sync)
            {
                return _gatewayClassData.ContainsKey(spec.GatewayClassName);
            }
        }

        return _isDefaultController;
    }

    private NamespaceCache Namespace(string key)
    {
        lock (_sync)
        {
            if (!_namespaceCaches.TryGetValue(key, out var value))
            {
                value = new NamespaceCache();
                _namespaceCaches.Add(key, value);
            }
            return value;
        }
    }
}