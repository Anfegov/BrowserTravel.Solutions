using BrowserTravel.Solutions.Domain.Entities;

namespace BrowserTravel.Solutions.Domain.Ports;
public interface ILocationRepository
{
    Task<Location> SaveLocation(Location location);
    Task<Location> GetLocationById(Guid uid);
    Task<List<Location>> GetAllLocation();
}
