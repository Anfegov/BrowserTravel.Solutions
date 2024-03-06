using BrowserTravel.Solutions.Domain.Dtos;
using BrowserTravel.Solutions.Domain.Entities;
using BrowserTravel.Solutions.Domain.Ports;
using MediatR;

namespace BrowserTravel.Solutions.Application.HistoryVehicle.Queries;

public class GetActiveServiceByVehicleIdQueryHandler : IRequestHandler<GetActiveServiceByVehicleIdQuery, HistoryVehicleDto>
{
    private readonly IHistoryVehicleRepository _repository;
    public GetActiveServiceByVehicleIdQueryHandler(IHistoryVehicleRepository repository) => _repository = repository;


    public async Task<HistoryVehicleDto> Handle(GetActiveServiceByVehicleIdQuery request, CancellationToken cancellationToken)
    {
        var historyVehicle = await _repository.GetServiceToVehicleById(request.Uid);
        return  new HistoryVehicleDto(historyVehicle.Id, historyVehicle.VehicleId, historyVehicle.OriginId, historyVehicle.DestinationId, historyVehicle.FullRoute);
    }
}
