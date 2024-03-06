using BrowserTravel.Solutions.Domain.Dtos;
using BrowserTravel.Solutions.Domain.Ports;
using MediatR;

namespace BrowserTravel.Solutions.Application.HistoryVehicle.Queries;

public class GetAvailabilityByOriginQueryHandler : IRequestHandler<GetAvailabilityByOriginQuery, List<VehicleDto>>
{
    private readonly IHistoryVehicleRepository _repository;
    public GetAvailabilityByOriginQueryHandler(IHistoryVehicleRepository repository) => _repository = repository;


    public async Task<List<VehicleDto>> Handle(GetAvailabilityByOriginQuery request, CancellationToken cancellationToken)
    {
        var vehicle = await _repository.GetVehicleAvailabilityByOrigin(request.Uid);
        return vehicle.Select(v => new VehicleDto(v.Id, v.Description, v.Model, v.Plate)).ToList();
    }
}
