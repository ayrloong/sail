namespace Sail.Compass.Services;

public interface IReconciler
{
    Task ProcessProxyAsync(CancellationToken cancellationToken);
    Task ProcessCertificateAsync(CancellationToken cancellationToken);
}