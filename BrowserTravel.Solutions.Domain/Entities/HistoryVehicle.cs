namespace BrowserTravel.Solutions.Domain.Entities
{
    public class HistoryVehicle : DomainEntity
    {
        public HistoryVehicle(
            Guid vehicleId,
            Guid originId,
            Guid destinationId,
            bool fullRoute)
        {
            VehicleId = vehicleId;
            OriginId = originId;
            DestinationId = destinationId;
            FullRoute = fullRoute;
        }

        public HistoryVehicle(
            Guid id,
            Guid vehicleId,
            Guid originId,
            Guid destinationId,
            bool fullRoute,
            DateTime createdOn)
        {
            Id = id; 
            VehicleId = vehicleId;
            OriginId = originId;
            DestinationId = destinationId;
            FullRoute = fullRoute;
            CreatedOn = createdOn;
        }

        public Guid VehicleId { get; init; }
        public Guid OriginId { get; init; }
        public Guid DestinationId { get; init; }
        public bool FullRoute { get; init; }
        public DateTime CreatedOn { get; init; }
        
        public virtual Location OriginNavigation { get; init; }
        public virtual Location DestinationNavigation { get; init; }
        public virtual Vehicle VehicleNavigation { get; init; }
    }
}
