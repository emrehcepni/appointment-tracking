using AppointmentTracking.CustomExtensions;
using AppointmentTracking.Domain.Entities;
using AppointmentTracking.Infrastructure.Repositories.Interfaces;
using AppointmentTracking.Services.Interfaces;

namespace AppointmentTracking.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;

    public AppointmentService(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<List<Appointment>> GetAppointments(int? month, int? week, Guid? instructorId)
    {
        var appointments = await _appointmentRepository.GetAppointments();

        if (month.HasValue)
            appointments = appointments.Where(a => a.StartTime.Month == month.Value);

        if (week.HasValue)
        {
            var startOfWeek = new DateTime().GetWeekStartDate(DateTime.Now.Year, month.Value, 1);
            var endOfWeek = startOfWeek.AddDays(6);
            appointments = appointments.Where(a => a.StartTime.Date >= startOfWeek && a.StartTime.Date <= endOfWeek);
        }

        if (instructorId.HasValue)
            appointments = appointments.Where(a => a.InstructorId == instructorId.Value);

        var result = appointments.ToList();
        return result;
    }
}
