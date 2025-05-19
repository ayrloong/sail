using Sail.Dashboard.Models.Clusters;

namespace Sail.Dashboard.Services;

public class ClusterService(DashboardClient client)
{
    public Task<List<ClusterResult>> ListAsync(CancellationToken cancellationToken)
    {
        return client.HttpClient.GetFromJsonAsync<List<ClusterResult>>("/api/clusters",
            cancellationToken: cancellationToken)!;
    }
    public Task<ClusterResult> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return client.HttpClient.GetFromJsonAsync<ClusterResult>($"/api/clusters/{id}",
            cancellationToken: cancellationToken)!;
    }
    public async Task CreateAsync(ClusterRequest request, CancellationToken cancellationToken)
    {
        var response =
            await client.HttpClient.PostAsJsonAsync("/api/clusters/", request, cancellationToken: cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateAsync(Guid id, ClusterRequest request, CancellationToken cancellationToken)
    {
        var response =
            await client.HttpClient.PutAsJsonAsync($"/api/clusters/{id}", request,
                cancellationToken: cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var response =
            await client.HttpClient.DeleteAsync($"/api/clusters/{id}", cancellationToken: cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}