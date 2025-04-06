using AppointmentTracking.Domain.Entities;

namespace AppointmentTracking.Services.Interfaces;

public interface IInstructorService
{
    public Task<IEnumerable<Instructor>> GetAllInstructors();
}
