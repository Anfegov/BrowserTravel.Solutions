using BrowserTravel.Solutions.Domain.Exceptions;

namespace BrowserTravel.Solutions.Domain.Entities
{
    public class Vehicle : DomainEntity
    {
        public Vehicle(
            string description,
            string model,
            string plate)
        {
            ValidateEmptyProperty(nameof(description), description);
            Description = description;
            Model = model;
            Plate = plate;
        }

        public Vehicle(
            Guid id,
            string description,
            string model,
            string plate)
        {
            ValidateEmptyProperty(nameof(description), description);
            Description = description;
            Model = model;
            Plate = plate;
        }

        private static void ValidateEmptyProperty(string propertyName, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new CoreException(DomainErrors.Errors.GenericError.IsNullOrWhiteSpace(propertyName));
            }
        }

        public string Description { get; init; }
        public string Model { get; init; }
        public string Plate { get; init; }
    }
}
