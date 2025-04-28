using AppointmentTracking.Domain.Entities;

namespace AppointmentTracking.Services.Interfaces;

public interface IInstructorService
{
    Task<IEnumerable<Instructor>> GetAllInstructors();
    Task<bool> DeleteInstructor(Guid instructorId);
    Task<List<Instructor>> SearchInstructors(string searchString);
}
