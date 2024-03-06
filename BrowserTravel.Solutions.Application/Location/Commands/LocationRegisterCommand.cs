using BrowserTravel.Solutions.Domain.Dtos;
using MediatR;

namespace BrowserTravel.Solutions.Application.Location.Commands;

public record LocationRegisterCommand(string Description) : IRequest<LocationDto>;
