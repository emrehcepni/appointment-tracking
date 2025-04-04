using AppointmentTracking.Domain.Entities;

namespace AppointmentTracking.Services.Interfaces;

public interface IAppointmentService
{
    public Task<List<Appointment>> GetAppointments(int? month, int? week, int? instructorId);
}
