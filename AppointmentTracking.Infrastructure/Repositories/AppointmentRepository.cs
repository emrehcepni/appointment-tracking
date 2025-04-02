using AppointmentTracking.Domain.Entities;
using AppointmentTracking.Infrastructure.Repositories.Interfaces;

namespace AppointmentTracking.Infrastructure.Repositories;

public class AppointmentRepository : GenericRepository<Appointment, Guid>, IAppointmentRepository
{
    private readonly AppDbContext _dbContext;

    public AppointmentRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
