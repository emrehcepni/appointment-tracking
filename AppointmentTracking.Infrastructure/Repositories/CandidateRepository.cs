using AppointmentTracking.Domain.Entities;
using AppointmentTracking.Infrastructure.Repositories.Interfaces;

namespace AppointmentTracking.Infrastructure.Repositories;

public class CandidateRepository : GenericRepository<Candidate, Guid>, ICandidateRepository
{
    private readonly AppDbContext _dbContext;

    public CandidateRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
