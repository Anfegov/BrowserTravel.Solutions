using BrowserTravel.Solutions.Domain.Dtos;
using MediatR;

namespace BrowserTravel.Solutions.Application.HistoryVehicle.Commands
{
    public record ConfirmEndOfServiceByIdCommand(Guid histotyServiceId) : IRequest<HistoryVehicleDto>;
}
