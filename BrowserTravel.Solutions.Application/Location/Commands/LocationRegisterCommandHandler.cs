using BrowserTravel.Solutions.Domain.Dtos;
using BrowserTravel.Solutions.Domain.Services;
using MediatR;

namespace BrowserTravel.Solutions.Application.Location.Commands;

public class LocationRegisterCommandHandler : IRequestHandler<LocationRegisterCommand, LocationDto>
{
    private readonly RecordLocationService _service;
    public LocationRegisterCommandHandler(RecordLocationService service)
    {
        _service = service;
    }

    public async Task<LocationDto> Handle(LocationRegisterCommand request, CancellationToken cancellationToken)
    {
        var locationSaved = await _service.RecordLocationAsync(
            new Domain.Entities.Location(request.Description), cancellationToken);

        return new LocationDto(locationSaved.Id, locationSaved.Description);

    }
}
