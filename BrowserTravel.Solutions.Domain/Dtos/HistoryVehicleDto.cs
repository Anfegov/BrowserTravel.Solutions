namespace BrowserTravel.Solutions.Domain.Dtos;

public record HistoryVehicleDto(Guid Id, Guid vehicleId, Guid originId, Guid destinationId, bool FullRoute);
