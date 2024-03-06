using BrowserTravel.Solutions.Domain.Dtos;
using BrowserTravel.Solutions.Domain.Services;
using MediatR;

namespace BrowserTravel.Solutions.Application.HistoryVehicle.Commands;

public class RegisterServiceVehicleCommandHandler : IRequestHandler<RegisterServiceVehicleCommand, HistoryVehicleDto>
{
    private readonly RecordHistoryVehicleService _service;
    public RegisterServiceVehicleCommandHandler(RecordHistoryVehicleService service)
    {
        _service = service;
    }

    public async Task<HistoryVehicleDto> Handle(RegisterServiceVehicleCommand request, CancellationToken cancellationToken)
    {
        var historyVehicleSaved = await _service.RecordHistoryVehicleAsync(
            new Domain.Entities.HistoryVehicle(request.vehicleId, request.originId, request.destinationId, false), cancellationToken);

        return new HistoryVehicleDto(historyVehicleSaved.Id, historyVehicleSaved.VehicleId, historyVehicleSaved.OriginId, historyVehicleSaved.DestinationId, historyVehicleSaved.FullRoute);

    }
}
