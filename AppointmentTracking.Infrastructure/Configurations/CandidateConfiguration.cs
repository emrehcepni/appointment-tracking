using AppointmentTracking.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AppointmentTracking.Domain.Constants;

namespace AppointmentTracking.Infrastructure.Configurations;

public class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
{
    public void Configure(EntityTypeBuilder<Candidate> builder)
    {
        builder.ToTable(TableNames.Candidates);

        builder.HasKey(c => c.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(c => c.IsDeleted).HasDefaultValue(true);
    }
}
