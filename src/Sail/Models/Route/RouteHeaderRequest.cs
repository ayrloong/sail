using Sail.Core.Entities;

namespace Sail.Models.Route;

public record RouteHeaderRequest(string Name, List<string> Values, HeaderMatchMode Mode, bool IsCaseSensitive);
