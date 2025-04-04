using AppointmentTracking.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AppointmentTracking.Domain.Constants;

namespace AppointmentTracking.Infrastructure.Configurations;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.ToTable(TableNames.Lessons);

        builder.HasKey(l => l.LessonId);
        builder.Property(l => l.IsDeleted).HasDefaultValue(true);
    }
}
