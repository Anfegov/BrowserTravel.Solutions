using MediatR;
using BrowserTravel.Solutions.Domain.Dtos;
using BrowserTravel.Solutions.Domain.Ports;

namespace BrowserTravel.Solutions.Application.Location.Queries;

public class LocationByIdQueryHandler : IRequestHandler<LocationByIdQuery, LocationDto>
{
    private readonly ILocationRepository _repository;
    public LocationByIdQueryHandler(ILocationRepository repository) => _repository = repository;


    public async Task<LocationDto> Handle(LocationByIdQuery request, CancellationToken cancellationToken)
    {
        var location = await _repository.GetLocationById(request.Uid);
        return new LocationDto(location.Id, location.Description);
    }
}
