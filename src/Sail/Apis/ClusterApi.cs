using Microsoft.AspNetCore.Http.HttpResults;
using Sail.Extensions;
using Sail.Models.Clusters;
using Sail.Services;

namespace Sail.Apis;

public static class ClusterApi
{
    public static RouteGroupBuilder MapClusterApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/clusters").HasApiVersion(1.0);

        api.MapGet("/", GetItems);
        api.MapPost("/", Create);
        api.MapPut("/{id:guid}", Update);
        api.MapDelete("/{id:guid}", Delete);
        return api;
    }

    private static async Task<Results<Ok<IEnumerable<ClusterResponse>>, NotFound>> GetItems(ClusterService service,
        CancellationToken cancellationToken)
    {
        var items = await service.GetAsync(cancellationToken);
        return TypedResults.Ok(items);
    }

    private static async Task<Results<Created, ProblemHttpResult>> Create(ClusterService service,
        ClusterRequest request, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(request, cancellationToken);

        return result.Match<Results<Created, ProblemHttpResult>>(
            created => TypedResults.Created(string.Empty),
            errors => errors.HandleErrors()
        );
    }

    private static async Task<Results<Ok, ProblemHttpResult>> Update(ClusterService service, Guid id,
        ClusterRequest request, CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(id, request, cancellationToken);

        return result.Match<Results<Ok, ProblemHttpResult>>(
            updated => TypedResults.Ok(),
            errors => errors.HandleErrors()
        );
    }

    private static async Task<Results<Ok, ProblemHttpResult>> Delete(ClusterService service, Guid id,
        CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(id, cancellationToken);

        return result.Match<Results<Ok, ProblemHttpResult>>(
            deleted => TypedResults.Ok(),
            errors => errors.HandleErrors()
        );
    }
}