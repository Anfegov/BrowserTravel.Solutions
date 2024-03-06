using BrowserTravel.Solutions.Domain.Dtos;
using BrowserTravel.Solutions.Domain.Ports;
using MediatR;

namespace BrowserTravel.Solutions.Application.Location.Queries;

public class LocationGetAllQueryHandler : IRequestHandler<LocationGetAllQuery, List<LocationDto>>
{
    private readonly ILocationRepository _repository;
    public LocationGetAllQueryHandler(ILocationRepository repository) => _repository = repository;


    public async Task<List<LocationDto>> Handle(LocationGetAllQuery request, CancellationToken cancellationToken)
    {
        var location = await _repository.GetAllLocation();

        return location.Select(v => new LocationDto(v.Id, v.Description)).ToList();
    }
}
