using MediatR;
using BrowserTravel.Solutions.Domain.Dtos;

namespace BrowserTravel.Solutions.Application.Vehicle.Queries;

public record VehicleByIdQuery(Guid Uid) : IRequest<VehicleDto>;
