using MediatR;
using BrowserTravel.Solutions.Domain.Dtos;

namespace BrowserTravel.Solutions.Application.Location.Queries;

public record LocationGetAllQuery() : IRequest<List<LocationDto>>;
