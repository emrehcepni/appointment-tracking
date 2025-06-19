using AppointmentTracking.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AppointmentTracking.Domain.Constants;

namespace AppointmentTracking.Infrastructure.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable(TableNames.Appointments);

        builder.HasKey(a => a.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(a => a.IsDeleted).HasDefaultValue(true);
        builder.HasOne(a => a.Vehicle)
            .WithMany(v => v.Appointments)
            .HasForeignKey(a => a.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(a => a.Instructor)
            .WithMany(i => i.Appointments)
            .HasForeignKey(a => a.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Candidate)
            .WithMany(c => c.Appointments)
            .HasForeignKey(a => a.CandidateId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
