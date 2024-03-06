using BrowserTravel.Solutions.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace BrowserTravel.Solutions.Infrastructure.DataSource;

public class DataContext : IdentityDbContext<IdentityUser>
{
    private readonly IConfiguration _config;
    private const string _schema_public = "public";
    public DataContext(DbContextOptions<DataContext> options, IConfiguration config) : base(options)
    {
        _config = config;
    }

    public DataContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (modelBuilder is null)
        {
            return;
        }
        modelBuilder.Entity<Vehicle>().ToTable(nameof(Vehicle), _schema_public);
        modelBuilder.Entity<Location>().ToTable(nameof(Location), _schema_public);
        modelBuilder.Entity<HistoryVehicle>().ToTable(nameof(HistoryVehicle), _schema_public);

        //modelBuilder.Entity<Vehicle>()
        //        .HasOne(v => v.LocationNavigation)
        //        .WithMany()
        //        .HasForeignKey(v => v.LocationId);

        modelBuilder.Entity<HistoryVehicle>()
            .HasOne(h => h.OriginNavigation)
            .WithMany()
            .HasForeignKey(h => h.OriginId);

        modelBuilder.Entity<HistoryVehicle>()
            .HasOne(h => h.DestinationNavigation)
            .WithMany()
            .HasForeignKey(h => h.DestinationId);

        modelBuilder.Entity<HistoryVehicle>()
            .HasOne(h => h.VehicleNavigation)
            .WithMany()
            .HasForeignKey(h => h.VehicleId);
        base.OnModelCreating(modelBuilder);
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var t = entityType.ClrType;
            if (typeof(DomainEntity).IsAssignableFrom(t))
            {
                modelBuilder.Entity(entityType.Name).Property<DateTime>("CreatedOn");
                modelBuilder.Entity(entityType.Name).Property<DateTime>("LastModifiedOn");
            }
        }


        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // Convertir el nombre de la tabla a snake_case
            entity.SetTableName(ConvertToSnakeCase(entity.GetTableName()));

            // Convertir los nombres de las columnas a snake_case
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(ConvertToSnakeCase(property.GetColumnName()));
            }

            // Convertir los nombres de las restricciones a snake_case
            foreach (var key in entity.GetKeys())
            {
                key.SetName(ConvertToSnakeCase(key.GetName()));
            }

            //Convertir los nombre de las llever foraneas
            foreach (var key in entity.GetDeclaredForeignKeys())
            {
                key.SetConstraintName(ConvertToSnakeCase(key.GetConstraintName()));
            }

            //Convertir los nombre de los index
            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(ConvertToSnakeCase(index.GetDatabaseName()));
            }
        }
    }

    private string ConvertToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        var snakeCaseChars = input.Select((x, i) =>
            i > 0 && char.IsUpper(x) &&
            (input[i - 1] != '_' && !char.IsUpper(input[i - 1]))
                ? "_" + x.ToString()
                : x.ToString());

        return string.Concat(snakeCaseChars).ToLower();
    }

    public virtual DbSet<Vehicle> Vehicle { get; set; }
    public virtual DbSet<Location> Location { get; set; }
    public virtual DbSet<HistoryVehicle> HistoryVehicle { get; set; }
    
}


