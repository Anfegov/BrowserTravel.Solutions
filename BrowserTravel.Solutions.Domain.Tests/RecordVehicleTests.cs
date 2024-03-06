using NSubstitute;
using BrowserTravel.Solutions.Api.Tests.Builder;
using BrowserTravel.Solutions.Domain.DomainErrors;
using BrowserTravel.Solutions.Domain.Exceptions;
using BrowserTravel.Solutions.Domain.Ports;
using BrowserTravel.Solutions.Domain.Services;
using BrowserTravel.Solutions.Domain.Entities;

namespace BrowserTravel.Solutions.Domain.Tests;
public class RecordVehicleTests
{
    readonly IVehicleRepository _repository = default!;
    readonly IUnitOfWork _unitOfWork;
    readonly RecordVehicleService _service = default!;

    public RecordVehicleTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _repository = Substitute.For<IVehicleRepository>();
        _service = new RecordVehicleService(_repository, _unitOfWork);
    }

    [Fact]
    public void RecordVehicleAsync_WithEmptyDescription_ThrowsUnderRangeException()
    {
        try
        {
            Vehicle vehicle = new VehicleBuilder().WithDescription("").BuildDomain();
            Assert.Fail("Should not get here");
        }
        catch (CoreException ex)
        {
            Assert.True(ex.Message.Equals(Errors.GenericError.IsNullOrWhiteSpace("description")));
        }
    }

    [Fact]
    public async void RecordVehicleAsync_WhenVehicleIsCorrectDescription_ShouldRecordVehicle()
    {
        Vehicle vehicle = new VehicleBuilder().BuildDomain();

        _repository.SaveVehicle(Arg.Any<Vehicle>()).Returns(vehicle);
        _unitOfWork.SaveAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        var result = await _service.RecordVehicleAsync(vehicle);

        await _repository.Received().SaveVehicle(Arg.Any<Vehicle>());
        await _unitOfWork.Received().SaveAsync(Arg.Any<CancellationToken>());

        Assert.Equal(vehicle.Id, result.Id);
        Assert.Equal(vehicle.Description, result.Description);
        Assert.Equal(vehicle.Model, result.Model);
        Assert.Equal(vehicle.Plate, result.Plate);
    }
}
