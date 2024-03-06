using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Serilog.Debugging;
using BrowserTravel.Solutions.Api;
using BrowserTravel.Solutions.Api.ApiHandlers;
using BrowserTravel.Solutions.Api.Filters;
using BrowserTravel.Solutions.Api.Middleware;
using BrowserTravel.Solutions.Api.Utilities;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Configuraci�n y servicios de validaci�n
builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);
builder.Services.AddPresentation(config);

// Configuraci�n de controladores y documentaci�n de API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Habilitar el registro de errores en la consola
SelfLog.Enable(Console.Error);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        // Configuraci�n de Identity
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Crear roles "admin" e "invitado"
        await DefaultUserAndRoles.SeedRolesAsync(roleManager);

        // Crear un usuario con el rol "admin"
        await DefaultUserAndRoles.SeedAdminUserAsync(userManager);
    }
    catch (Exception ex)
    {
        // Manejar excepciones seg�n sea necesario
        Console.WriteLine(ex.Message);
    }
}

// Configuraci�n para entornos de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configuraci�n de autenticaci�n y autorizaci�n
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

// Middleware para manejar excepciones de la aplicaci�n
app.UseMiddleware<AppExceptionHandlerMiddleware>();

// Mapeo de controladores y grupos de API con filtros de validaci�n
app.MapControllers(); 
app.MapGroup("/api/user").MapUser().AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory);
app.MapGroup("/api/role").MapRole().AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory);
app.MapGroup("/api/vehicle").MapVehicle().AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory);
app.MapGroup("/api/location").MapLocation().AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory);
app.MapGroup("/api/historyVehicle").MapHistoryVehicle().AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory);

app.Run();