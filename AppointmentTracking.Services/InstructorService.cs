using AppointmentTracking.Domain.Entities;
using AppointmentTracking.Infrastructure.Repositories.Interfaces;
using AppointmentTracking.Services.Interfaces;

namespace AppointmentTracking.Services;

public class InstructorService : IInstructorService
{
    private IInstructorRepository _instructorRepository;

    public InstructorService(IInstructorRepository instructorRepository)
    {
        _instructorRepository = instructorRepository;
    }

    public async Task<IEnumerable<Instructor>> GetAllInstructors()
    {
        var instructors = await _instructorRepository.WhereWithAsNoTrackingAsync(item => !item.IsDeleted);
        return instructors;
    }
    
    public async Task<bool> DeleteInstructor(Guid instructorId)
    {
        try
        {
            await _instructorRepository.DeleteByIdAsync(instructorId, Guid.Empty);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<Instructor>> SearchInstructors(string searchString)
    {
        var instructors = await _instructorRepository.WhereWithAsNoTrackingAsync(i =>
            !i.IsDeleted && (
                i.FirstName.Contains(searchString) ||
                i.LastName.Contains(searchString) ||
                i.PhoneNumber.Contains(searchString) ||
                i.LicanceType.Contains(searchString)
            ));
    
        return instructors.ToList();
    }
    
    public async Task<bool> AddInstructor(Instructor instructor)
    {
        var result = await _instructorRepository.CreateAsync(instructor);
        return result.Id != Guid.Empty;
    }
    
    public async Task<Instructor?> GetInstructorById(Guid instructorId)
    {
        var instructor = await _instructorRepository.FirstOrDefaultWithAsNoTrackingAsync(item => item.Id == instructorId && !item.IsDeleted);
        return instructor;
    }
    
    public async Task UpdateInstructor(Instructor instructor)
    {
        await _instructorRepository.UpdateAsync(instructor);
    }
}
