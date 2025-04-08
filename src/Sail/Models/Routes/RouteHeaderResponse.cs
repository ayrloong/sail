using Sail.Core.Entities;

namespace Sail.Models.Routes;

public record RouteHeaderResponse
{
    public string Name { get; init; }
    public List<string> Values { get; init; }
    public HeaderMatchMode Mode { get; init; }
    public bool IsCaseSensitive { get; init; }
}