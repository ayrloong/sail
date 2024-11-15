using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Sail.Core.Certificates;
using Sail.Core.Converters;
using Sail.Core.Options;
using Sail.Core.Stores;

namespace Sail.Core.Hosting;

public class CertificateHostedService(
    IServerCertificateSelector serverCertificateSelector,
    IServiceScopeFactory serviceScopeFactory,
    IOptions<DefaultOptions> options) : BackgroundService
{

    private readonly DefaultOptions _options = options.Value ?? throw new ArgumentNullException(nameof(options));

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new(TimeSpan.FromSeconds(_options.InitializePeriodTime));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await UpdateAsync(stoppingToken);
        }
    }

    private async Task UpdateAsync(CancellationToken cancellationToken)
    {
        try
        {
            await using var scope = serviceScopeFactory.CreateAsyncScope();
            var certificateStore =
                scope.ServiceProvider.GetService<ICertificateStore>() ?? throw new NullReferenceException();

            var certificates = await certificateStore.GetAsync(cancellationToken);
            var config = Parser.ConvertCertificates(certificates);

            await serverCertificateSelector.UpdateAsync(config, cancellationToken);

        }
        catch (Exception e)
        {
            // ignored
        }
    }
}