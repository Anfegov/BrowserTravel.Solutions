using BrowserTravel.Solutions.Domain.Dtos;
using BrowserTravel.Solutions.Domain.Services;
using MediatR;

namespace BrowserTravel.Solutions.Application.Vehicle.Commands;

public class VehicleRegisterCommandHandler : IRequestHandler<VehicleRegisterCommand, VehicleDto>
{
    private readonly RecordVehicleService _service;
    public VehicleRegisterCommandHandler(RecordVehicleService service)
    {
        _service = service;
    }

    public async Task<VehicleDto> Handle(VehicleRegisterCommand request, CancellationToken cancellationToken)
    {
        var vehicleSaved = await _service.RecordVehicleAsync(
            new Domain.Entities.Vehicle(request.Description, request.Model, request.Plate), cancellationToken);

        return new VehicleDto(vehicleSaved.Id, vehicleSaved.Description, vehicleSaved.Model, vehicleSaved.Plate);

    }
}
