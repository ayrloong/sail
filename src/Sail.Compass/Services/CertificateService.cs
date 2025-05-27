using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sail.Compass.Hosting;

namespace Sail.Compass.Services;

public class CertificateService : BackgroundHostedService
{
    public CertificateService(IHostApplicationLifetime hostApplicationLifetime, ILogger logger) : base(
        hostApplicationLifetime, logger)
    {
        
    }

    public override Task RunAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}