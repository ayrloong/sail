using Sail.Core.Entities;

namespace Sail.Models.Routes;

public record QueryParameterRequest(
    string Name,
    List<string> Values,
    QueryParameterMatchMode Mode,
    bool IsCaseSensitive);