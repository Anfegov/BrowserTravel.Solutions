using MediatR;
using BrowserTravel.Solutions.Domain.Dtos;

namespace BrowserTravel.Solutions.Application.Location.Queries;

public record LocationByIdQuery(Guid Uid) : IRequest<LocationDto>;
