using Microsoft.AspNetCore.Http.HttpResults;
using Sail.Extensions;
using Sail.Models.Certificates;
using Sail.Services;

namespace Sail.Apis;

public static class CertificateApi
{
    public static RouteGroupBuilder MapCertificateApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/certificates").HasApiVersion(1.0);

        api.MapGet("/", GetItems);
        api.MapPost("/", Create);
        api.MapPatch("/", Update);
        api.MapDelete("/{id:guid}", Delete);

        api.MapGet("/{certificateId:guid}/snis", GetSNIs);
        api.MapPost("/{certificateId:guid}/snis", CreateSNI);
        api.MapPost("/{certificateId:guid}/snis/{id:guid}", UpdateSNI);
        api.MapDelete("/{certificateId:guid}/snis/{id:guid}", DeleteSNI);
        return api;
    }

    private static async Task<Results<Ok<IEnumerable<SNIResponse>>, NotFound>> GetSNIs(CertificateService service,
        Guid certificateId,
        CancellationToken cancellationToken)
    {
        var items = await service.GetSNIsAsync(certificateId, cancellationToken);
        return TypedResults.Ok(items);
    }

    private static async Task<Results<Created, ProblemHttpResult>> CreateSNI(CertificateService service,
        Guid certificateId, SNIRequest request, CancellationToken cancellationToken)
    {
        var result = await service.CreateSNIAsync(certificateId, request, cancellationToken);

        return result.Match<Results<Created, ProblemHttpResult>>(
            created => TypedResults.Created(string.Empty),
            errors => errors.HandleErrors()
        );
    }

    private static async Task<Results<Ok, ProblemHttpResult>> UpdateSNI(CertificateService service, Guid certificateId,
        Guid id, SNIRequest request, CancellationToken cancellationToken)
    {
        var result = await service.UpdateSNIAsync(certificateId, id, request, cancellationToken);

        return result.Match<Results<Ok, ProblemHttpResult>>(
            created => TypedResults.Ok(),
            errors => errors.HandleErrors()
        );
    }

    private static async Task<Results<Ok, ProblemHttpResult>> DeleteSNI(CertificateService service, Guid certificateId,
        Guid id, CancellationToken cancellationToken)
    {
        var result = await service.DeleteSNIAsync(certificateId, id, cancellationToken);

        return result.Match<Results<Ok, ProblemHttpResult>>(
            created => TypedResults.Ok(),
            errors => errors.HandleErrors()
        );
    }

    private static async Task<Results<Ok<IEnumerable<CertificateResponse>>, NotFound>> GetItems(
        CertificateService service,
        CancellationToken cancellationToken)
    {
        var items = await service.GetAsync(cancellationToken);
        return TypedResults.Ok(items);
    }

    private static async Task<Results<Created, ProblemHttpResult>> Create(CertificateService service,
        CertificateRequest request, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(request, cancellationToken);

        return result.Match<Results<Created, ProblemHttpResult>>(
            created => TypedResults.Created(string.Empty),
            errors => errors.HandleErrors()
        );
    }

    private static async Task<Results<Ok, ProblemHttpResult>> Update(CertificateService service, Guid id,
        CertificateRequest request, CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(id, request, cancellationToken);

        return result.Match<Results<Ok, ProblemHttpResult>>(
            updated => TypedResults.Ok(),
            errors => errors.HandleErrors()
        );
    }

    private static async Task<Results<Ok, ProblemHttpResult>> Delete(CertificateService service, Guid id,
        CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(id, cancellationToken);

        return result.Match<Results<Ok, ProblemHttpResult>>(
            deleted => TypedResults.Ok(),
            errors => errors.HandleErrors()
        );
    }
}