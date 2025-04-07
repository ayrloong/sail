using Sail.Core.Entities;

namespace Sail.Models.Route;

public record RouteHeaderVm
{
    public string Name { get; init; }
    public List<string> Values { get; init; }
    public HeaderMatchMode Mode { get; init; }
    public bool IsCaseSensitive { get; init; }
}