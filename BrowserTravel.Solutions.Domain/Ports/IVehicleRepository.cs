using BrowserTravel.Solutions.Domain.Entities;

namespace BrowserTravel.Solutions.Domain.Ports;
public interface IVehicleRepository
{
    Task<Vehicle> SaveVehicle(Vehicle vehicle);
    Task<Vehicle> GetVehicleById(Guid uid);
    Task<List<Vehicle>> GetAllVehicle();
}
