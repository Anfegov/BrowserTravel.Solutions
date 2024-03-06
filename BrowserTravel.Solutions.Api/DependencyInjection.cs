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
    // Método de extensión para configurar la inyección de dependencias relacionadas con la presentación
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        // Agregar servicios del dominio
        services.AddDomainServices();

        // Configuración de Entity Framework y Identity
        services.AddEntityFrameworkNpgsql()
            .AddDbContext<DataContext>(
                opt => opt.UseNpgsql(configuration.GetConnectionString("DB"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking))
            .AddIdentityCore<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

        // Configuración de HttpContextAccessor, autorización y autenticación JWT
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

        // Configuración de Swagger para la documentación de la API
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "BrowserTravel.Solutions.Api",
                Version = "v1"
            });

            // Configuración del esquema de seguridad Bearer para Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            // Configuración de requisitos de seguridad para Swagger
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

        // Configuración de MediatR para manejar la lógica de aplicación
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("BrowserTravel.Solutions.Application")));

        return services;
    }
}
