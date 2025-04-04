using Microsoft.EntityFrameworkCore;
using AppointmentTracking.Domain.Entities;
using Microsoft.Extensions.Logging;

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
        modelBuilder.HasDefaultSchema("dbo");
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=DESKTOP-BBG06F2;Database=DireksiyonTakipDb;Trusted_Connection=True; TrustServerCertificate=True;")
            .LogTo(Console.WriteLine, LogLevel.Information);
    }
}
