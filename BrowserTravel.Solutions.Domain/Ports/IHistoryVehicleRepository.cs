using BrowserTravel.Solutions.Domain.Entities;
using System.Data.Common;

namespace BrowserTravel.Solutions.Domain.Ports;
public interface IHistoryVehicleRepository
{
    Task<HistoryVehicle> SaveHistoryVehicle(HistoryVehicle historyVehicle);
    Task<HistoryVehicle> GetHistoryVehicleById(Guid uid);
    void UpdateHistoryVehicle(HistoryVehicle historyVehicle);
    Task<List<Vehicle>> GetVehicleAvailabilityByOrigin(Guid origin);
    Task<HistoryVehicle> GetServiceToVehicleById(Guid vehicleId);
}
