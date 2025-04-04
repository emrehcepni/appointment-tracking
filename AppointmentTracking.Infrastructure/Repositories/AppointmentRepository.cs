using AppointmentTracking.Domain.Entities;
using AppointmentTracking.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppointmentTracking.Infrastructure.Repositories;

public class AppointmentRepository : GenericRepository<Appointment, Guid>, IAppointmentRepository
{
    private readonly AppDbContext _dbContext;

    public AppointmentRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IQueryable<Appointment>> GetAppointments()
    {
        var appointments = _dbContext.Appointments
            .Include(a => a.Instructor)
            .Include(a => a.Candidate)
            .Include(a => a.Vehicle)
            .Where(item => !item.IsDeleted)
            .AsNoTracking()
            .AsQueryable();

        return appointments;
    }
}
