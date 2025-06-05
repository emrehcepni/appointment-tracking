using AppointmentTracking.Domain.Entities;

namespace AppointmentTracking.Services.Interfaces;

public interface ICandidateService
{
    Task<List<Candidate>> GetAllCandidates();
    
    // Task<bool> DeleteCandidate(Guid candidateId);
    // Task<List<Candidate>> SearchCandidate(string searchString);
    // Task<bool> AddCandidate(Candidate candidate);
    // Task<Candidate?> GetCandidateById(Guid candidateId);
    // Task UpdateCandidate(Candidate candidate);
    // Task<int> CountCandidate(int candidateId);
    //public Task<IEnumerable<Candidate>> GetAllCandidates();
}
