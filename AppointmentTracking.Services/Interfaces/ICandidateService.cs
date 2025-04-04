using AppointmentTracking.Domain.Entities;

namespace AppointmentTracking.Services.Interfaces;

public interface ICandidateService
{
    public Task<IEnumerable<Candidate>> GetAllCandidates();
}
