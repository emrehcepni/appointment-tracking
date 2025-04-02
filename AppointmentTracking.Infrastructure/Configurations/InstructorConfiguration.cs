using AppointmentTracking.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AppointmentTracking.Infrastructure.Configurations;

public class InstractorConfiguration : IEntityTypeConfiguration<Instructor>
{
    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
        builder.HasKey(i => i.InstructorId);
        builder.Property(i => i.IsDeleted).HasDefaultValue(true);
    }
}
