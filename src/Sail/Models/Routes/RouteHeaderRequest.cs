using Sail.Core.Entities;

namespace Sail.Models.Routes;

public record RouteHeaderRequest(string Name, List<string> Values, HeaderMatchMode Mode, bool IsCaseSensitive);
