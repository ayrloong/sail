using k8s;
using k8s.Autorest;
using k8s.Models;

namespace Sail.Kubernetes.Controller.Client;

internal class V1ServiceResourceInformer(
    IKubernetes client,
    ResourceSelector<V1Service> selector,
    IHostApplicationLifetime hostApplicationLifetime,
    ILogger<V1ServiceResourceInformer> logger)
    : ResourceInformer<V1Service, V1ServiceList>(client, selector, hostApplicationLifetime, logger)
{
    protected override Task<HttpOperationResponse<V1ServiceList>> RetrieveResourceListAsync(bool? watch = null,
        string resourceVersion = null, ResourceSelector<V1Service> resourceSelector = null,
        CancellationToken cancellationToken = default)
    {
        return Client.CoreV1.ListServiceForAllNamespacesWithHttpMessagesAsync(watch: watch,
            resourceVersion: resourceVersion, fieldSelector: resourceSelector?.FieldSelector,
            cancellationToken: cancellationToken);
    }
}