namespace Sail.Models.Certificates;

public record CertificateResponse
{
    public Guid Id { get; init; }
    public string Cert { get; init; }
    public string Key { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
}