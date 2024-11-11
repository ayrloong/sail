using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Sail.Protocol.Extensions;
using Sail.Protocol.Services;
using Route = Sail.Core.Entities.Route;

namespace Sail.Protocol.Apis;

public static class RouteApi
{
    public static RouteGroupBuilder MapRouteApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/routes");
        
        api.MapGet("/", GetItems);
        api.MapPost("/", Create);
        api.MapPatch("/", Update);
        api.MapDelete("/{id:guid}", Delete);
        return api;
    }

    private static async Task<Results<Ok<IEnumerable<Route>>, NotFound>> GetItems(IRouteService service,
        CancellationToken cancellationToken)
    {
        var items = await service.GetAsync();
        return TypedResults.Ok(items);
    }

    private static async Task<Results<Created, ProblemHttpResult>> Create(IRouteService service)
    {
        var result = await service.CreateAsync();

        return result.Match<Results<Created, ProblemHttpResult>>(
            created => TypedResults.Created(string.Empty),
            errors => errors.HandleErrors()
        );
    }

    private static async Task<Results<Ok, ProblemHttpResult>> Update(IRouteService service)
    {
        var result = await service.UpdateAsync();

        return result.Match<Results<Ok, ProblemHttpResult>>(
            created => TypedResults.Ok(),
            errors => errors.HandleErrors()
        );
    }

    private static async Task<Results<Ok, ProblemHttpResult>> Delete(IRouteService service,Guid id)
    {
        var result = await service.DeleteAsync(id);

        return result.Match<Results<Ok, ProblemHttpResult>>(
            created => TypedResults.Ok(),
            errors => errors.HandleErrors()
        );
    }
}

public abstract record CreateRouteRequest(Guid ClusterId, string Name, RouteMatchRequest Match);
public abstract record RouteMatchRequest(string Path,List<string> Methods, List<string> Hosts);