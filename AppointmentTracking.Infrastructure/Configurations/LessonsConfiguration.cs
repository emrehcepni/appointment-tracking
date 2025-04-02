using AppointmentTracking.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AppointmentTracking.Infrastructure.Configurations;

public class LessonsConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.HasKey(l => l.LessonId);
        builder.Property(l => l.IsDeleted).HasDefaultValue(true);
    }
}
