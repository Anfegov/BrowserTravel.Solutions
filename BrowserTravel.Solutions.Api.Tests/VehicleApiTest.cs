using BrowserTravel.Solutions.Api.Tests.Builder;
using BrowserTravel.Solutions.Application.Vehicle.Commands;
using BrowserTravel.Solutions.Domain.Dtos;
using BrowserTravel.Solutions.Domain.Entities;
using BrowserTravel.Solutions.Domain.Ports;
using BrowserTravel.Solutions.Infrastructure.Ports;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BrowserTravel.Solutions.Api.Tests;

public class VehicleApiTest
{
    [Fact]
    public async Task GetSingleVehicleSuccess()
    {
        await using var webApp = new ApiApp();
        var serviceCollection = webApp.GetServiceCollection();

        using var scope = serviceCollection.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IRepository<Vehicle>>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var vehicle = new VehicleBuilder().BuildDomain();
        var vehicleCreated = await repository.AddAsync(new Vehicle(vehicle.Description, vehicle.Model, vehicle.Plate) { Id = webApp.UserId });

        await unitOfWork.SaveAsync(new CancellationTokenSource().Token);

        var client = webApp.CreateClient();

        var request = await client.GetAsync("/api/user/Login?userName=Administrador1&password=Administrador1%21");
        request.EnsureSuccessStatusCode();

        var deserializeOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var responseData = System.Text.Json.JsonSerializer.Deserialize<UserLoginDto>(await request.Content.ReadAsStringAsync(), deserializeOptions);

       client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseData?.AccessToken);

        var singleVehicle = await client.GetFromJsonAsync<VehicleDto>($"/api/vehicle/getById?id={vehicleCreated.Id}");

        Assert.True(singleVehicle is not null && singleVehicle is VehicleDto);
        Assert.Equal(singleVehicle.Id, webApp.UserId);
    }

    [Fact]
    public async Task PostVehicleSuccess()
    {
        await using var webApp = new ApiApp();

        var client = webApp.CreateClient();

        var request1 = await client.GetAsync("/api/user/Login?userName=Administrador1&password=Administrador1%21");
        request1.EnsureSuccessStatusCode();

        var deserializeOptions1 = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var responseData1 = System.Text.Json.JsonSerializer.Deserialize<UserLoginDto>(await request1.Content.ReadAsStringAsync(), deserializeOptions1);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseData1?.AccessToken);

        var request = await client.PostAsJsonAsync<VehicleRegisterCommand>("/api/vehicle/", new VehicleBuilder().BuildCommand());
        request.EnsureSuccessStatusCode();

        var deserializeOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var responseData = System.Text.Json.JsonSerializer.Deserialize<VehicleDto>(await request.Content.ReadAsStringAsync(), deserializeOptions);

        Assert.True(responseData is not null);
        Assert.IsType<VehicleDto>(responseData);
    }

    [Fact]
    public async Task PostVehicleFailureByDescription()
    {
        HttpResponseMessage request = default!;

        try
        {
            await using var webApp = new ApiApp();

            VehicleRegisterCommand vehicle = new VehicleBuilder().WithDescription("").BuildCommand();

            var client = webApp.CreateClient();

            var request1 = await client.GetAsync("/api/user/Login?userName=Administrador1&password=Administrador1%21");
            request1.EnsureSuccessStatusCode();

            var deserializeOptions1 = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var responseData1 = System.Text.Json.JsonSerializer.Deserialize<UserLoginDto>(await request1.Content.ReadAsStringAsync(), deserializeOptions1);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseData1?.AccessToken);

            request = await client.PostAsJsonAsync<VehicleRegisterCommand>("/api/vehicle", vehicle);
            request.EnsureSuccessStatusCode();

            Assert.Fail("There is no way to get here if the description is empty");
        }
        catch (Exception)
        {
            var responseMessage = (await request.Content.ReadAsStringAsync()).Replace("\\u0027", "'").Replace("\\u00F3", "ó").Replace("\\u00ED","í");
            Assert.True(request.StatusCode is HttpStatusCode.BadRequest && responseMessage.Contains(BrowserTravel.Solutions.Domain.DomainErrors.Errors.GenericError.IsNullOrWhiteSpace("description")));
        }
    }
}

