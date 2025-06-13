using System.Collections.Immutable;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sail.Compass.Hosting;
using Sail.Compass.Rate;

namespace Sail.Compass.Informers;

public abstract class ResourceInformer<TResource>(IHostApplicationLifetime hostApplicationLifetime, ILogger logger)
    : BackgroundHostedService(hostApplicationLifetime, logger), IResourceInformer<TResource> where TResource : class
{
    private readonly Lock _sync = new();
    private readonly SemaphoreSlim _ready = new(0);
    private readonly SemaphoreSlim _start = new(0);
    private ImmutableList<Registration> _registrations = ImmutableList<Registration>.Empty;

    public override async Task RunAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _start.WaitAsync(cancellationToken).ConfigureAwait(false);

            var limiter = new Limiter(new Limit(0.2), 3);

            var list = GetObservable(false);
            list.Subscribe(InvokeRegistrationCallbacks,
                error => { },
                () => { _ready.Release(); });

            var watch = GetObservable(true);
            watch.Subscribe(InvokeRegistrationCallbacks,
                error => { });

            await limiter.WaitAsync(cancellationToken).ConfigureAwait(true);

        }
        catch (Exception error)
        {
        }
    }

    private void InvokeRegistrationCallbacks(ResourceEvent<TResource> resource)
    {
        List<Exception> innerExceptions = default;
        foreach (var registration in _registrations)
        {
            try
            {
                registration.Callback.Invoke(resource);
            }
            catch (Exception innerException)
            {
                if (innerExceptions is null)
                {
                    innerExceptions = [];
                }

                innerExceptions.Add(innerException);
            }
        }

        if (innerExceptions is not null)
        {
            throw new AggregateException("One or more exceptions thrown by ResourceInformerCallback.", innerExceptions);
        }
    }

    protected abstract IObservable<ResourceEvent<TResource>> GetObservable(bool watch);
    
    public void StartWatching()
    {
        _start.Release();
    }

    public async Task ReadyAsync(CancellationToken cancellationToken)
    {
        await _ready.WaitAsync(cancellationToken).ConfigureAwait(false);
        
        _ready.Release();
    }
    
    public virtual IResourceInformerRegistration Register(ResourceInformerCallback<TResource> callback)
    {
        return new Registration(this, callback);
    }
    
    internal class Registration : IResourceInformerRegistration
    {
        private bool _disposedValue;

        public Registration(ResourceInformer<TResource> resourceInformer, ResourceInformerCallback<TResource> callback)
        {
            ResourceInformer = resourceInformer;
            Callback = callback;
            lock (resourceInformer._sync)
            {
                resourceInformer._registrations = resourceInformer._registrations.Add(this);
            }
        }

        ~Registration()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public ResourceInformer<TResource> ResourceInformer { get; }
        public ResourceInformerCallback<TResource> Callback { get; }

        public Task ReadyAsync(CancellationToken cancellationToken) => ResourceInformer.ReadyAsync(cancellationToken);

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                lock (ResourceInformer._sync)
                {
                    ResourceInformer._registrations = ResourceInformer._registrations.Remove(this);
                }
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}