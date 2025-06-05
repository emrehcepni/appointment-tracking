using AppointmentTracking.Domain.Entities;

namespace AppointmentTracking.Services.Interfaces;

public interface ICandidateService
{
    Task<IQueryable<Candidate>> GetAllCandidates(); //IEnumerable yazdığımda verileri önceden çektiği için asnotracking hata veriyordu. bunu sor
    Task<bool> DeleteCandidate(Guid candidateId);
    Task<List<Candidate>> SearchCandidate(string searchString);
    Task<bool> AddCandidate(Candidate candidate);
    Task<Candidate?> GetCandidateById(Guid candidateId);
    Task UpdateCandidate(Candidate candidate);

    Task<int> CountCandidate(int candidateId);




    //public Task<IEnumerable<Candidate>> GetAllCandidates();
}
