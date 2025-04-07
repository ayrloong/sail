using System.Reactive.Linq;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sail.Api.V1;

namespace Sail.Compass.Informers;

public class V1CertificateResourceInformer(
    IHostApplicationLifetime hostApplicationLifetime,
    CertificateService.CertificateServiceClient client,
    ILogger<V1CertificateResourceInformer> logger) : ResourceInformer<Certificate>(hostApplicationLifetime, logger)
{
    protected override IObservable<ResourceEvent<Certificate>> GetObservable(bool watch)
    {
        var result = watch ? Watch() : List();
        return result;
    }

    private IObservable<ResourceEvent<Certificate>> List()
    {
        return Observable.Create<ResourceEvent<Certificate>>((observer, cancellationToken) =>
        {
            var list = client.List(new Empty(), cancellationToken: cancellationToken);
            foreach (var item in list.Items)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;
                var resource = item.ToResourceEvent(EventType.List);
                observer.OnNext(resource!);
            }

            observer.OnCompleted();
            return Task.CompletedTask;
        });
    }

    private IObservable<ResourceEvent<Certificate>> Watch()
    {
        return Observable.Create<ResourceEvent<Certificate>>(async (observer, cancellationToken) =>
        {
            var result = client.Watch(new Empty(), cancellationToken: cancellationToken);
            var watch = result.ResponseStream;
            await foreach (var current in watch.ReadAllAsync(cancellationToken: cancellationToken))
            {
                var eventType = current.EventType switch
                {
                    Api.V1.EventType.Create => EventType.Created,
                    Api.V1.EventType.Update => EventType.Updated,
                    Api.V1.EventType.Delete => EventType.Deleted,
                    _ => EventType.Unknown,
                };

                var resource = current.Certificate.ToResourceEvent(eventType);
                observer.OnNext(resource!);
            }
        });
    }
}