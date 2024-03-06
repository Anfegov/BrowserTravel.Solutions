using MediatR;
using BrowserTravel.Solutions.Domain.Dtos;

namespace BrowserTravel.Solutions.Application.HistoryVehicle.Queries;

public record GetActiveServiceByVehicleIdQuery(Guid Uid) : IRequest<HistoryVehicleDto>;
