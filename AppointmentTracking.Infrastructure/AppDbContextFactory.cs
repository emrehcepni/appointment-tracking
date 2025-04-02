using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AppointmentTracking.Infrastructure
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-BBG06F2;Database=DireksiyonTakipDb;Trusted_Connection=True; TrustServerCertificate=True;");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
