using BrowserTravel.Solutions.Api.Tests.Builder;
using BrowserTravel.Solutions.Application.Location.Commands;
using BrowserTravel.Solutions.Domain.Dtos;
using BrowserTravel.Solutions.Domain.Ports;
using BrowserTravel.Solutions.Infrastructure.Ports;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BrowserTravel.Solutions.Api.Tests;

public class LocationApiTest
{
    [Fact]
    public async Task GetSingleLocationSuccess()
    {
        await using var webApp = new ApiApp();
        var serviceCollection = webApp.GetServiceCollection();

        using var scope = serviceCollection.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IRepository<BrowserTravel.Solutions.Domain.Entities.Location>>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var location = new LocationBuilder().BuildDomain();
        var locationCreated = await repository.AddAsync(new BrowserTravel.Solutions.Domain.Entities.Location(location.Description) { Id = webApp.UserId });

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

        var singleLocation = await client.GetFromJsonAsync<LocationDto>($"/api/location/getById?id={locationCreated.Id}");

        Assert.True(singleLocation is not null && singleLocation is LocationDto);
        Assert.Equal(singleLocation.Id, webApp.UserId);
    }

    [Fact]
    public async Task PostLocationSuccess()
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

        var request = await client.PostAsJsonAsync<LocationRegisterCommand>("/api/location/", new LocationBuilder().BuildCommand());
        request.EnsureSuccessStatusCode();

        var deserializeOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var responseData = System.Text.Json.JsonSerializer.Deserialize<LocationDto>(await request.Content.ReadAsStringAsync(), deserializeOptions);

        Assert.True(responseData is not null);
        Assert.IsType<LocationDto>(responseData);
    }

    [Fact]
    public async Task PostLocationFailureByDescription()
    {
        HttpResponseMessage request = default!;

        try
        {
            await using var webApp = new ApiApp();

            LocationRegisterCommand location = new LocationBuilder().WithDescription("").BuildCommand();

            var client = webApp.CreateClient();

            var request1 = await client.GetAsync("/api/user/Login?userName=Administrador1&password=Administrador1%21");
            request1.EnsureSuccessStatusCode();

            var deserializeOptions1 = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var responseData1 = System.Text.Json.JsonSerializer.Deserialize<UserLoginDto>(await request1.Content.ReadAsStringAsync(), deserializeOptions1);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseData1?.AccessToken);

            request = await client.PostAsJsonAsync<LocationRegisterCommand>("/api/location", location);
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

