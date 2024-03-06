using BrowserTravel.Solutions.Infrastructure.DataSource;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace BrowserTravel.Solutions.Api.Tests;

// Clase de prueba que hereda de WebApplicationFactory para la aplicación API
class ApiApp : WebApplicationFactory<Program>
{
    // Identificador único para el usuario de la prueba
    readonly Guid _id;

    // Propiedad que expone el identificador único del usuario de la prueba
    public Guid UserId => this._id;

    // Constructor de la clase ApiApp
    public ApiApp()
    {
        // Generar un nuevo identificador único para cada instancia de la aplicación de prueba
        _id = Guid.NewGuid();
    }

    // Método para obtener el contenedor de servicios (IServiceProvider) de la aplicación de prueba
    public IServiceProvider GetServiceCollection()
    {
        return Services;
    }

    // Método para crear el host de la aplicación de prueba
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(svc =>
        {
            // Reemplazar y agregar configuraciones de servicios específicos de la prueba
            svc.RemoveAll(typeof(DbContextOptions<DataContext>));
            svc.AddDbContext<DataContext>(opt =>
            {
                opt.UseInMemoryDatabase("testdb");
            });
        });

        // Llamar al método base para crear el host
        return base.CreateHost(builder);
    }
}
