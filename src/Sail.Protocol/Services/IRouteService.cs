using ErrorOr;
using Sail.Core.Entities;

namespace Sail.Protocol.Services;

public interface IRouteService
{
    Task<IEnumerable<Route>> GetAsync();
    Task<ErrorOr<Created>> CreateAsync();
    Task<ErrorOr<Updated>> UpdateAsync();
    Task<ErrorOr<Deleted>> DeleteAsync(Guid id);
}