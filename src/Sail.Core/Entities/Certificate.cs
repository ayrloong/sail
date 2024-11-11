namespace Sail.Core.Entities;

public class Certificate
{
    public Guid Id { get; set; }
    public string Cert { get; set; }
    public string Key { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}