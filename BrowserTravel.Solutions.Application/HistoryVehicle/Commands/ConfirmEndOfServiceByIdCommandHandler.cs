using BrowserTravel.Solutions.Domain.Dtos;
using BrowserTravel.Solutions.Domain.Ports;
using BrowserTravel.Solutions.Domain.Services;
using MediatR;

namespace BrowserTravel.Solutions.Application.HistoryVehicle.Commands;

public class ConfirmEndOfServiceByIdCommandHandler : IRequestHandler<ConfirmEndOfServiceByIdCommand, HistoryVehicleDto>
{
    private readonly UpdateHistoryVehicleService _service;
    private readonly IHistoryVehicleRepository _repository;
    public ConfirmEndOfServiceByIdCommandHandler(UpdateHistoryVehicleService service, IHistoryVehicleRepository repository)
    {
        _service = service;
        _repository = repository;
    }

    public async Task<HistoryVehicleDto> Handle(ConfirmEndOfServiceByIdCommand request, CancellationToken cancellationToken)
    {
        var historyVehicule = await _repository.GetHistoryVehicleById(request.histotyServiceId);
        var historyVehicleUpdate = new Domain.Entities.HistoryVehicle(historyVehicule.Id, historyVehicule.VehicleId, historyVehicule.OriginId, historyVehicule.DestinationId, true, DateTime.UtcNow);

        var historyVehicleSaved = await _service.UpdateHistoryVehicleAsync(historyVehicleUpdate/*
            new Domain.Entities.HistoryVehicle(historyVehicule.Id, historyVehicule.OriginId, historyVehicule.DestinationId, true, DateTime.UtcNow)*/, cancellationToken);

        return new HistoryVehicleDto(historyVehicleSaved.Id, historyVehicleSaved.VehicleId, historyVehicleSaved.OriginId, historyVehicleSaved.DestinationId, historyVehicleSaved.FullRoute);

    }
}
