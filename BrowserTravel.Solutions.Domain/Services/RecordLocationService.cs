using BrowserTravel.Solutions.Domain.Entities;
using BrowserTravel.Solutions.Domain.Exceptions;
using BrowserTravel.Solutions.Domain.Ports;

namespace BrowserTravel.Solutions.Domain.Services;

[DomainService]
public class RecordLocationService
{
    private readonly ILocationRepository _locationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RecordLocationService(
        ILocationRepository locationRepository,
        IUnitOfWork unitOfWork) =>
        (_locationRepository, _unitOfWork) = (locationRepository, unitOfWork);

    public async Task<Location> RecordLocationAsync(Location location, CancellationToken? cancellationToken = null)
    {
        if (location == null)
        {
            throw new CoreException(DomainErrors.Errors.GenericError.IsNullValue(nameof(location)));
        }
        var token = cancellationToken ?? new CancellationTokenSource().Token;
        var returnLocation = await _locationRepository.SaveLocation(location);
        await _unitOfWork.SaveAsync(token);
        return returnLocation;
    }
}
