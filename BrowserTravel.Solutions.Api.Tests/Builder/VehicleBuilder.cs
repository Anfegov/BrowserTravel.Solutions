using BrowserTravel.Solutions.Application.Vehicle.Commands;
using BrowserTravel.Solutions.Domain.Entities;

namespace BrowserTravel.Solutions.Api.Tests.Builder;

public class VehicleBuilder
{
    private string _description = "Kia picanto";
    private string _model = "2023";
    private string _plate = "cnc123";

    public VehicleBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }
    public VehicleBuilder WithModel(string model)
    {
        _model = model;
        return this;
    }

    public VehicleBuilder WithPlace(string place)
    {
        _plate = place;
        return this;
    }

    public VehicleRegisterCommand BuildCommand()
    {
        return new VehicleRegisterCommand(_description, _model, _plate);
    }

    public Vehicle BuildDomain()
    {
        return new Vehicle(_description, _model, _plate);
    }
}
