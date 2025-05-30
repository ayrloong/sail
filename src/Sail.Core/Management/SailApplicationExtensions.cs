using Microsoft.Extensions.DependencyInjection;
using Sail.Core.Options;

namespace Sail.Core.Management;

public static class SailApplicationExtensions
{
    public static SailApplication AddSailCore(this IServiceCollection services)
    {
        var app = new SailApplication(services);
        services.AddConfiguration();
        return app;
    }

    private static void AddConfiguration(this IServiceCollection services)
    {
        services.AddOptions<DatabaseOptions>().BindConfiguration(nameof(DatabaseOptions.Name));
        services.AddOptions<CertificateOptions>().BindConfiguration(nameof(CertificateOptions.Name));
        services.AddOptions<ConsulOptions>().BindConfiguration(nameof(ConsulOptions.Name));
        services.AddOptions<ReceiverOptions>().BindConfiguration(nameof(ReceiverOptions.Name));
    }
}