using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using BrowserTravel.Solutions.Infrastructure.DataSource;
using BrowserTravel.Solutions.Infrastructure.Extensions;

namespace BrowserTravel.Solutions.Api;

public static class DependencyInjection
{
    // M�todo de extensi�n para configurar la inyecci�n de dependencias relacionadas con la presentaci�n
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        // Agregar servicios del dominio
        services.AddDomainServices();

        // Configuraci�n de Entity Framework y Identity
        services.AddEntityFrameworkNpgsql()
            .AddDbContext<DataContext>(
                opt => opt.UseNpgsql(configuration.GetConnectionString("DB"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking))
            .AddIdentityCore<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

        // Configuraci�n de HttpContextAccessor, autorizaci�n y autenticaci�n JWT
        services.AddHttpContextAccessor()
            .AddAuthorization()
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });

        // Configuraci�n de Swagger para la documentaci�n de la API
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "BrowserTravel.Solutions.Api",
                Version = "v1"
            });

            // Configuraci�n del esquema de seguridad Bearer para Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            // Configuraci�n de requisitos de seguridad para Swagger
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });

        // Configuraci�n de MediatR para manejar la l�gica de aplicaci�n
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("BrowserTravel.Solutions.Application")));

        return services;
    }
}
