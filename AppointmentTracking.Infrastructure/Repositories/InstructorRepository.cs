using AppointmentTracking.Domain.Entities;
using AppointmentTracking.Infrastructure.Repositories.Interfaces;

namespace AppointmentTracking.Infrastructure.Repositories;

public class InstructorRepository : GenericRepository<Instructor, Guid>, IInstructorRepository
{
    private readonly AppDbContext _dbContext;

    public InstructorRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
