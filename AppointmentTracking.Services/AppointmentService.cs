using AppointmentTracking.CustomExtensions;
using AppointmentTracking.Domain.Entities;
using AppointmentTracking.Infrastructure.Repositories.Interfaces;
using AppointmentTracking.Services.Interfaces;

namespace AppointmentTracking.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly ICandidateRepository _candidateRepository;

    public AppointmentService(IAppointmentRepository appointmentRepository,IVehicleRepository vehicleRepository, ICandidateRepository candidateRepository)
    {
        _appointmentRepository = appointmentRepository;
        _vehicleRepository = vehicleRepository;
        _candidateRepository = candidateRepository;
    }

    public async Task<List<Appointment>> GetAppointments(int? month, int? week, Guid? instructorId, Guid? vehicleId)
    {
        var appointments = await _appointmentRepository.GetAppointments();

        if (month.HasValue)
            appointments = appointments.Where(a => a.StartTime.Month == month.Value);

        if (week.HasValue)
        {
            var startOfWeek = new DateTime().GetWeekStartDate(DateTime.Now.Year, month ?? 1, week.Value);
            var endOfWeek = startOfWeek.AddDays(6);
            appointments = appointments.Where(a => a.StartTime.Date >= startOfWeek && a.StartTime.Date <= endOfWeek);
        }

        if (instructorId.HasValue)
            appointments = appointments.Where(a => a.InstructorId == instructorId.Value);

        if (vehicleId.HasValue)
            appointments = appointments.Where(a => a.VehicleId == vehicleId.Value);

        return appointments.ToList();
    }
    public async Task<bool> SaveAppointment(Guid instructorId, Guid candidateId, Guid vehicleId, DateTime startTime)
    {
        var appointment = new Appointment
        {
            InstructorId = instructorId,
            CandidateId = candidateId,
            VehicleId = vehicleId,
            StartTime = startTime,
            CreatedDate = DateTime.UtcNow
        };

        await _appointmentRepository.CreateAsync(appointment);
        return true;
    }
    public async Task<List<Vehicle>> GetAllVehicles()
    {
        var vehicles = await _vehicleRepository.WhereWithAsNoTrackingAsync(v => v.Accessible && !v.IsDeleted);
        return vehicles.ToList();
    }

    public async Task<List<Candidate>> GetAllCandidates()
    {
        var candidates = await _candidateRepository.WhereWithAsNoTrackingAsync(c => !c.IsDeleted);
        return candidates.ToList();
    }
    public async Task<bool> DeleteAppointment(Guid appointmentId)
    {
        var appointment = await _appointmentRepository.FirstOrDefaultAsync(a => a.Id == appointmentId);
        if (appointment == null)
            return false;

        appointment.IsDeleted = true;
        await _appointmentRepository.UpdateAsync(appointment);
        return true;
    }
    public async Task<List<Vehicle>> GetAvailableVehicles(DateTime startTime, DateTime endTime)
    {
        var allVehicles = await _vehicleRepository.WhereWithAsNoTrackingAsync(v => v.Accessible && !v.IsDeleted);

        var appointments = await _appointmentRepository.GetAppointments();
        var bookedVehicleIds = appointments
            .Where(a =>
                a.StartTime < endTime &&
                a.StartTime.AddHours(1) > startTime) // Çakışan randevular
            .Select(a => a.VehicleId)
            .Distinct()
            .ToList();

        var availableVehicles = allVehicles
            .Where(v => !bookedVehicleIds.Contains(v.Id))
            .ToList();

        return availableVehicles;
    }
    public async Task<bool> HasVehicleConflict(Guid vehicleId, DateTime startTime, DateTime endTime)
    {
        var appointments = await _appointmentRepository.GetAppointments();
        return appointments.Any(a =>
            a.VehicleId == vehicleId &&
            a.StartTime < endTime &&
            a.StartTime.AddHours(1) > startTime && // varsayılan süre 1 saat
            !a.IsDeleted
        );
    }

}
