using Sail.Dashboard.Models.Routes;

namespace Sail.Dashboard.Services;

public class RouteService(DashboardClient client)
{
    public Task<List<RouteResult>> ListAsync(CancellationToken cancellationToken)
    {
        return client.HttpClient.GetFromJsonAsync<List<RouteResult>>("/api/routes",
            cancellationToken: cancellationToken)!;
    }
    public Task<RouteResult> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return client.HttpClient.GetFromJsonAsync<RouteResult>($"/api/routes/{id}",
            cancellationToken: cancellationToken)!;
    }
    public async Task CreateAsync(RouteRequest request, CancellationToken cancellationToken)
    {
        var response =
            await client.HttpClient.PostAsJsonAsync("/api/routes/", request, cancellationToken: cancellationToken);
        response.EnsureSuccessStatusCode();
    }
    public async Task UpdateAsync(Guid id, RouteRequest request, CancellationToken cancellationToken)
    {
        var response =
            await client.HttpClient.PutAsJsonAsync($"/api/routes/{id}", request,
                cancellationToken: cancellationToken);
        response.EnsureSuccessStatusCode();
    }
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var response =
            await client.HttpClient.DeleteAsync($"/api/routes/{id}", cancellationToken: cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}