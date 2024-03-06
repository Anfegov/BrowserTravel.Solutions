using BrowserTravel.Solutions.Domain.Entities;
using BrowserTravel.Solutions.Domain.Exceptions;
using BrowserTravel.Solutions.Domain.Ports;

namespace BrowserTravel.Solutions.Domain.Services;

[DomainService]
public class RecordHistoryVehicleService
{
    private readonly IHistoryVehicleRepository _historyVehicleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RecordHistoryVehicleService(
        IHistoryVehicleRepository historyVehicleRepository,
        IUnitOfWork unitOfWork) =>
        (_historyVehicleRepository, _unitOfWork) = (historyVehicleRepository, unitOfWork);

    public async Task<HistoryVehicle> RecordHistoryVehicleAsync(HistoryVehicle historyVehicle, CancellationToken? cancellationToken = null)
    {
        if (historyVehicle == null)
        {
            throw new CoreException(DomainErrors.Errors.GenericError.IsNullValue(nameof(historyVehicle)));
        }
        var token = cancellationToken ?? new CancellationTokenSource().Token;
        var returnHistoryVehicle = await _historyVehicleRepository.SaveHistoryVehicle(historyVehicle);
        await _unitOfWork.SaveAsync(token);
        return returnHistoryVehicle;
    }
}
