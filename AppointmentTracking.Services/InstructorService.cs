using AppointmentTracking.Domain.Entities;
using AppointmentTracking.Infrastructure.Repositories;
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
}
