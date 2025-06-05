using AppointmentTracking.Domain.Entities;
using AppointmentTracking.Infrastructure.Repositories.Interfaces;
using AppointmentTracking.Services.Interfaces;

namespace AppointmentTracking.Services;

public class CandidateService : ICandidateService
{
    private ICandidateRepository _candidateRepository;

    public CandidateService(ICandidateRepository candidateRepository)
    {
        _candidateRepository = candidateRepository;
    }

    public async Task<List<Candidate>> GetAllCandidates()
    {
        var candidates = await _candidateRepository.WhereWithAsNoTrackingAsync(item => !item.IsDeleted);
        return candidates.ToList();
    }
}
