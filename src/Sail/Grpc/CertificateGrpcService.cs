using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Sail.Api.V1;
using Sail.Core.Stores;
using Certificate = Sail.Core.Entities.Certificate;
using CertificateResponse = Sail.Api.V1.Certificate;

namespace Sail.Grpc;

public class CertificateGrpcService(ICertificateStore certificateStore)
    : CertificateService.CertificateServiceBase
{
    public override async Task StreamCertificates(CertificateRequest request,
        IServerStreamWriter<CertificateItems> responseStream, ServerCallContext context)
    {
        while (!context.CancellationToken.IsCancellationRequested)
        {
            var certifications = await certificateStore.GetAsync(CancellationToken.None);
            var response = MapToDiscoveryResponse(certifications);

            await responseStream.WriteAsync(response);
            await Task.Delay(TimeSpan.FromSeconds(20));
        }
    }

    private static CertificateItems MapToDiscoveryResponse(List<Certificate> certificates)
    {
        var items = certificates.Select(MapToCertificateResponse);
       
        var response = new CertificateItems
        { 
            Items = { items }
        };
        return response;
    }

    private static  CertificateResponse MapToCertificateResponse(Certificate certificate)
    {
        return new CertificateResponse
        {
            CertificateId = certificate.Id.ToString(),
            Key = certificate.Key,
            Value = certificate.Cert,
            Hosts = { certificate.SNIs.Select(item => item.HostName).ToArray() }
        };
    }
}