using AppointmentTracking.Domain.Entities;

namespace AppointmentTracking.Services.Interfaces;

public interface IAppointmentService
{
    Task<List<Appointment>> GetAppointments(int? month, int? week, Guid? instructorId);
}
