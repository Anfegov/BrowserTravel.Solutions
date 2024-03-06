using BrowserTravel.Solutions.Domain.Dtos;
using MediatR;

namespace BrowserTravel.Solutions.Application.HistoryVehicle.Commands
{
    public record RegisterServiceVehicleCommand(Guid vehicleId,Guid originId, Guid destinationId) : IRequest<HistoryVehicleDto>;
}
