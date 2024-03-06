using BrowserTravel.Solutions.Domain.Entities;
using BrowserTravel.Solutions.Domain.Ports;
using BrowserTravel.Solutions.Infrastructure.Ports;

namespace BrowserTravel.Solutions.Infrastructure.Adapters;

[Repository]
public class LocationRepository : ILocationRepository
{
    readonly IRepository<Location> _dataSource;
    public LocationRepository(IRepository<Location> dataSource) => _dataSource = dataSource
        ?? throw new ArgumentNullException(nameof(dataSource));

    public async Task<Location> GetLocationById(Guid uid) => await _dataSource.GetOneAsync(uid);

    public async Task<Location> SaveLocation(Location location) => await _dataSource.AddAsync(location);
    public async Task<List<Location>> GetAllLocation() => (List<Location>)await _dataSource.GetManyAsync();
}
