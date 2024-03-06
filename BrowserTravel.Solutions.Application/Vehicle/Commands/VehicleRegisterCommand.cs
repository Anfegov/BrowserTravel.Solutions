using BrowserTravel.Solutions.Domain.Dtos;
using MediatR;

namespace BrowserTravel.Solutions.Application.Vehicle.Commands;

public record VehicleRegisterCommand(string Description, string Model, string Plate) : IRequest<VehicleDto>;
