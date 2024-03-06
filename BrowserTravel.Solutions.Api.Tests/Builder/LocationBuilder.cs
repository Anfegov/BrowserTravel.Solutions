using BrowserTravel.Solutions.Application.Location.Commands;

namespace BrowserTravel.Solutions.Api.Tests.Builder;

public class LocationBuilder
{
    private string _description = "punto Tulua";

    public LocationBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }
  
    public LocationRegisterCommand BuildCommand()
    {
        return new LocationRegisterCommand(_description);
    }

    public BrowserTravel.Solutions.Domain.Entities.Location BuildDomain()
    {
        return new BrowserTravel.Solutions.Domain.Entities.Location(_description);
    }
}
