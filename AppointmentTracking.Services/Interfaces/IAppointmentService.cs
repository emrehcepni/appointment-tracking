using AppointmentTracking.Domain.Entities;

namespace AppointmentTracking.Services.Interfaces;

public interface IAppointmentService
{
    Task<bool> SaveAppointment(Guid instructorId, Guid candidateId, Guid vehicleId, DateTime startTime);
    Task<List<Appointment>> GetAppointments(int? month, int? week, Guid? instructorId, Guid? vehicleId);
    Task<List<Vehicle>> GetAllVehicles();
    Task<List<Candidate>> GetAllCandidates();
    Task<bool> DeleteAppointment(Guid appointmentId);
    Task<List<Vehicle>> GetAvailableVehicles(DateTime startTime, DateTime endTime);
    Task<bool> HasVehicleConflict(Guid vehicleId, DateTime startTime, DateTime endTime);

}
