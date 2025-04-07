using Sail.Core.Entities;

namespace Sail.Models.Route;

public record QueryParameterRequest(
    string Name,
    List<string> Values,
    QueryParameterMatchMode Mode,
    bool IsCaseSensitive);