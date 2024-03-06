using MediatR;
using BrowserTravel.Solutions.Domain.Dtos;

namespace BrowserTravel.Solutions.Application.HistoryVehicle.Queries;

public record GetAvailabilityByOriginQuery(Guid Uid) : IRequest<List<VehicleDto>>;
