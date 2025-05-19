namespace Sail.Dashboard;

public class DashboardClient(HttpClient httpClient)
{
    public HttpClient HttpClient => httpClient;
}