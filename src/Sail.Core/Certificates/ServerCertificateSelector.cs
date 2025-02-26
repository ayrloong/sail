using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Options;
using Sail.Core.Options;

namespace Sail.Core.Certificates;

public class ServerCertificateSelector(IOptions<CertificateOptions> options)
    : IServerCertificateSelector
{
    private volatile IReadOnlyList<CertificateConfig> _certificates = new List<CertificateConfig>();
    private readonly CertificateOptions _options = options.Value;

    public X509Certificate2 GetCertificate(ConnectionContext connectionContext, string domainName)
    {
        var certificate = _certificates.SingleOrDefault(x => x.HostName == domainName);

        if (certificate is null)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return X509Certificate2.CreateFromPemFile(_options.DefaultPath, _options.DefaultKeyPath);
            {
                using var convertedCertificate =
                    X509Certificate2.CreateFromPemFile(_options.DefaultPath, _options.DefaultKeyPath);
                return X509CertificateLoader.LoadCertificate(convertedCertificate.Export(X509ContentType.Pkcs12));
            }
        }

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return X509Certificate2.CreateFromPem(certificate.Cert, certificate.Key);
        {
            using var convertedCertificate = X509Certificate2.CreateFromPem(certificate.Cert, certificate.Key);
            return X509CertificateLoader.LoadCertificate(convertedCertificate.Export(X509ContentType.Pkcs12));
        }
    }

    public Task UpdateAsync(IReadOnlyList<CertificateConfig> certificates, CancellationToken cancellationToken)
    {
        Interlocked.Exchange(ref _certificates, certificates);
        return Task.CompletedTask;
    }
}