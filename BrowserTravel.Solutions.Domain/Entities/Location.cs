using BrowserTravel.Solutions.Domain.Exceptions;

namespace BrowserTravel.Solutions.Domain.Entities
{
    public class Location : DomainEntity
    {
        public Location(
            string description)
        {
            ValidateEmptyProperty(nameof(description), description);
            Description = description;
        }

        private static void ValidateEmptyProperty(string propertyName, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new CoreException(DomainErrors.Errors.GenericError.IsNullOrWhiteSpace(propertyName));
            }
        }

        public string Description { get; init; }
        public DateTime CreationDate { get; init; }

    }
}
