using Microsoft.EntityFrameworkCore;
using AppointmentTracking.Domain.Entities;

namespace AppointmentTracking.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    #region DbSets
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Candidate> Candidates { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<VehicleDetail> VehicleDetails { get; set; }
    public DbSet<User> Users { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("public");
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }
}
