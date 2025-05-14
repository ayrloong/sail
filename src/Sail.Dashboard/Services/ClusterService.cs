using Sail.Dashboard.Models.Clusters;

namespace Sail.Dashboard.Services;

public class ClusterService(DashboardClient client)
{
    public Task<List<ClusterResponse>> GetClustersAsync(CancellationToken cancellationToken)
    {
        return client.HttpClient.GetFromJsonAsync<List<ClusterResponse>>("/api/clusters",
            cancellationToken: cancellationToken)!;
    }

    public async Task CreateClusterAsync(ClusterRequest request, CancellationToken cancellationToken)
    {
        var response =
            await client.HttpClient.PostAsJsonAsync("/api/clusters/", request, cancellationToken: cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateClusterAsync(Guid id, ClusterRequest request, CancellationToken cancellationToken)
    {
        var response =
            await client.HttpClient.PutAsJsonAsync($"/api/clusters/{id}", request,
                cancellationToken: cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteClusterAsync(Guid id, CancellationToken cancellationToken)
    {
        var response =
            await client.HttpClient.DeleteAsync($"/api/clusters/{id}", cancellationToken: cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}