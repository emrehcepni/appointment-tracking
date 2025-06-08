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
    public async Task<bool> DeleteCandidate(Guid candidateId)
    {
        var candidate = await _candidateRepository.GetById(candidateId);
        if (candidate == null) return false;

        candidate.IsDeleted = true;
        await _candidateRepository.UpdateAsync(candidate);
        return true;
    }

    public async Task<List<Candidate>> SearchCandidate(string searchString)
    {
        var candidates = await _candidateRepository.WhereWithAsNoTrackingAsync(c =>
            !c.IsDeleted &&
            (c.FirstName.Contains(searchString) ||
             c.LastName.Contains(searchString) ||
             c.PhoneNumber.Contains(searchString)));

        return candidates.ToList();
    }
    public async Task<bool> AddCandidate(Candidate candidate)
    {
        if (candidate == null) return false;

        await _candidateRepository.CreateAsync(candidate);
        return true;
    }
    public async Task<Candidate?> GetCandidateById(Guid candidateId)
    {
        return await _candidateRepository.GetById(candidateId);
    }
    public async Task UpdateCandidate(Candidate candidate)
    {
        await _candidateRepository.UpdateAsync(candidate);
    }
    public async Task<int> CountCandidate(Guid candidateId)
    {
        var candidates = await _candidateRepository
            .WhereWithAsNoTrackingAsync(c => c.Id == candidateId && !c.IsDeleted);

        return candidates.Count();
    }
}
