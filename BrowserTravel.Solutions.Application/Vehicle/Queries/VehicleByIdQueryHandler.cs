using MediatR;
using BrowserTravel.Solutions.Domain.Dtos;
using BrowserTravel.Solutions.Domain.Ports;

namespace BrowserTravel.Solutions.Application.Vehicle.Queries;

public class VehicleByIdQueryHandler : IRequestHandler<VehicleByIdQuery, VehicleDto>
{
    private readonly IVehicleRepository _repository;
    public VehicleByIdQueryHandler(IVehicleRepository repository) => _repository = repository;


    public async Task<VehicleDto> Handle(VehicleByIdQuery request, CancellationToken cancellationToken)
    {
        var vehicle = await _repository.GetVehicleById(request.Uid);
        return new VehicleDto(vehicle.Id, vehicle.Description, vehicle.Model, vehicle.Plate);
    }
}
