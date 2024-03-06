using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrowserTravel.Solutions.Domain.Entities;

namespace BrowserTravel.Solutions.Infrastructure.DataSource.ModelConfig;

public class VehicleEntityTypeConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.Property(b => b.Id).IsRequired();
        builder.Property(b => b.Id)
        .HasConversion(
            v => v,               
            v => v);
    }
}

