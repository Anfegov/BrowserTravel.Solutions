using BrowserTravel.Solutions.Domain.Dtos;
using BrowserTravel.Solutions.Domain.Entities;
using BrowserTravel.Solutions.Domain.Ports;
using BrowserTravel.Solutions.Infrastructure.DataSource;
using BrowserTravel.Solutions.Infrastructure.Ports;
using Microsoft.EntityFrameworkCore;

namespace BrowserTravel.Solutions.Infrastructure.Adapters;

[Repository]
public class HistoryVehicleRepository : IHistoryVehicleRepository
{
    private readonly IRepository<HistoryVehicle> _dataSource;
    private readonly DataContext _context;
    public HistoryVehicleRepository(IRepository<HistoryVehicle> dataSource, DataContext context) =>
        (_dataSource, _context) = (dataSource, context);

    public async Task<HistoryVehicle> GetHistoryVehicleById(Guid uid) => await _dataSource.GetOneAsync(uid);

    public async Task<List<Vehicle>> GetVehicleAvailabilityByOrigin(Guid origin)
    {
        var rankedHistory = _context.HistoryVehicle
                                    .OrderByDescending(h => h.CreatedOn)
                                    .GroupBy(h => h.VehicleId);

        var mostRecentHistories = rankedHistory
                                    .AsEnumerable()
                                    .SelectMany(group => group
                                    .Select((h, index) => new
                                    {
                                        h.VehicleId,
                                        h.DestinationId,
                                        h.FullRoute,
                                        Rn = index + 1
                                    }))
                                    .GroupBy(h => h.VehicleId)
                                    .Select(group => group.OrderByDescending(h => h.Rn).FirstOrDefault())
                                    .Where(h => h != null)
                                    .ToList();

        var list = mostRecentHistories
                                    .Where(m => m.FullRoute == true && m.DestinationId == origin)
                                    .Select(m => m.VehicleId)
                                    .ToList();

        return await _context.Vehicle
                                    .Where(v => list.Contains(v.Id))
                                    .ToListAsync();
    }

    public async Task<HistoryVehicle> GetServiceToVehicleById(Guid vehicleId)
    {
        var rankedHistory = _context.HistoryVehicle
                                    .OrderByDescending(h => h.CreatedOn)
                                    .GroupBy(h => h.VehicleId);

        var mostRecentHistories = rankedHistory
                                    .AsEnumerable()
                                    .SelectMany(group => group
                                    .Select((h, index) => new
                                    {
                                        h.Id,
                                        h.VehicleId,
                                        h.DestinationId,
                                        h.FullRoute,
                                        Rn = index + 1
                                    }))
                                    .GroupBy(h => h.VehicleId)
                                    .Select(group => group.OrderByDescending(h => h.Rn).FirstOrDefault())
                                    .Where(h => h != null)
                                    .ToList();

        var list = mostRecentHistories
                                    .Where(m => m.FullRoute == false && m.VehicleId == vehicleId)
                                    .Select(m => m.Id)
                                    .ToList();

        return await _context.HistoryVehicle
                                    .Where(v => list.Contains(v.Id))
                                    .FirstOrDefaultAsync();
    }

    public async Task<HistoryVehicle> SaveHistoryVehicle(HistoryVehicle historyVehicle) => await _dataSource.AddAsync(historyVehicle);
     public void UpdateHistoryVehicle(HistoryVehicle historyVehicle) => _dataSource.UpdateAsync(historyVehicle);
}
