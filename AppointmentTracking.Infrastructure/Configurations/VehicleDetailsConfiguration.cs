using AppointmentTracking.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AppointmentTracking.Domain.Constants;

namespace AppointmentTracking.Infrastructure.Configurations;

public class VehicleDetailsConfiguration : IEntityTypeConfiguration<VehicleDetail>
{
    public void Configure(EntityTypeBuilder<VehicleDetail> builder)
    {
        builder.ToTable(TableNames.VehicleDetails);

        builder.HasKey(v => v.VehicleDetailId);
        builder.Property(v => v.IsDeleted).HasDefaultValue(true);

        builder.HasOne(vd => vd.Vehicle)
            .WithOne(v => v.VehicleDetails)
            .HasForeignKey<VehicleDetail>(vd => vd.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
