using BrowserTravel.Solutions.Domain.Entities;
using BrowserTravel.Solutions.Domain.Exceptions;
using BrowserTravel.Solutions.Domain.Ports;

namespace BrowserTravel.Solutions.Domain.Services;

[DomainService]
public class UpdateHistoryVehicleService
{
    private readonly IHistoryVehicleRepository _historyVehicleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateHistoryVehicleService(
        IHistoryVehicleRepository historyVehicleRepository,
        IUnitOfWork unitOfWork) =>
        (_historyVehicleRepository, _unitOfWork) = (historyVehicleRepository, unitOfWork);

    public async Task<HistoryVehicle> UpdateHistoryVehicleAsync(HistoryVehicle historyVehicle, CancellationToken? cancellationToken = null)
    {
            if (historyVehicle == null)
            {
                throw new CoreException(DomainErrors.Errors.GenericError.IsNullValue(nameof(historyVehicle)));
            }
            var token = cancellationToken ?? new CancellationTokenSource().Token;
            _historyVehicleRepository.UpdateHistoryVehicle(historyVehicle);
            await _unitOfWork.SaveAsync(token);
            return historyVehicle;
    }
}