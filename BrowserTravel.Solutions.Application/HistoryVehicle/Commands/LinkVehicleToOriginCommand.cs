using BrowserTravel.Solutions.Domain.Dtos;
using MediatR;

namespace BrowserTravel.Solutions.Application.HistoryVehicle.Commands
{
    public record LinkVehicleToOriginCommand(Guid vehicleId,Guid originId) : IRequest<HistoryVehicleDto>;
}
