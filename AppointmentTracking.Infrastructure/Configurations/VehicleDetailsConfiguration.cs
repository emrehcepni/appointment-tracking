using AppointmentTracking.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AppointmentTracking.Infrastructure.Configurations;

public class VehicleDetailsConfiguration : IEntityTypeConfiguration<VehicleDetails>
{
    public void Configure(EntityTypeBuilder<VehicleDetails> builder)
    {
        builder.HasKey(v => v.VehicleDetailId);
        builder.Property(v => v.IsDeleted).HasDefaultValue(true);

        builder.HasOne(vd => vd.Vehicle)
            .WithOne(v => v.VehicleDetails)
            .HasForeignKey<VehicleDetails>(vd => vd.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
