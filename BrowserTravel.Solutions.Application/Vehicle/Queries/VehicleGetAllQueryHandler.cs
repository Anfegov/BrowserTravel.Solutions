using BrowserTravel.Solutions.Domain.Dtos;
using BrowserTravel.Solutions.Domain.Ports;
using MediatR;

namespace BrowserTravel.Solutions.Application.Vehicle.Queries;

public class VehicleGetAllQueryHandler : IRequestHandler<VehicleGetAllQuery, List<VehicleDto>>
{
    private readonly IVehicleRepository _repository;
    public VehicleGetAllQueryHandler(IVehicleRepository repository) => _repository = repository;


    public async Task<List<VehicleDto>> Handle(VehicleGetAllQuery request, CancellationToken cancellationToken)
    {
        var vehicle = await _repository.GetAllVehicle();

        return vehicle.Select(v => new VehicleDto(v.Id, v.Description, v.Model, v.Plate)).ToList();
    }
}
