using AppointmentTracking.Domain.Constants;
using AppointmentTracking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentTracking.Infrastructure.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable(TableNames.Vehicles);

        builder.HasKey(v => v.VehicleId);
        builder.Property(v => v.IsDeleted).HasDefaultValue(true);
    }
}
