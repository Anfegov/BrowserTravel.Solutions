using BrowserTravel.Solutions.Domain.Entities;
using BrowserTravel.Solutions.Domain.Exceptions;
using BrowserTravel.Solutions.Domain.Ports;

namespace BrowserTravel.Solutions.Domain.Services;

[DomainService]
public class RecordVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RecordVehicleService(
        IVehicleRepository vehicleRepository,
        IUnitOfWork unitOfWork) =>
        (_vehicleRepository, _unitOfWork) = (vehicleRepository, unitOfWork);

    public async Task<Vehicle> RecordVehicleAsync(Vehicle vehicle, CancellationToken? cancellationToken = null)
    {
        if (vehicle == null)
        {
            throw new CoreException(DomainErrors.Errors.GenericError.IsNullValue(nameof(vehicle)));
        }
        var token = cancellationToken ?? new CancellationTokenSource().Token;
        var returnVehicle = await _vehicleRepository.SaveVehicle(vehicle);
        await _unitOfWork.SaveAsync(token);
        return returnVehicle;
    }
}
