using BrowserTravel.Solutions.Domain.Entities;
using BrowserTravel.Solutions.Domain.Ports;
using BrowserTravel.Solutions.Infrastructure.Ports;

namespace BrowserTravel.Solutions.Infrastructure.Adapters;

[Repository]
public class VehicleRepository : IVehicleRepository
{
    readonly IRepository<Vehicle> _dataSource;
    public VehicleRepository(IRepository<Vehicle> dataSource) => _dataSource = dataSource
        ?? throw new ArgumentNullException(nameof(dataSource));

    public async Task<Vehicle> GetVehicleById(Guid uid) => await _dataSource.GetOneAsync(uid);

    public async Task<Vehicle> SaveVehicle(Vehicle vehicle) => await _dataSource.AddAsync(vehicle);
    public async Task<List<Vehicle>> GetAllVehicle() => (List<Vehicle>)await _dataSource.GetManyAsync();
}
