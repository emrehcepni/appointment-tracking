using AppointmentTracking.Domain.Entities;

namespace AppointmentTracking.Services.Interfaces;

public interface IAppointmentService
{
    Task<bool> SaveAppointment(Guid instructorId, Guid candidateId, Guid vehicleId, DateTime startTime);
    Task<List<Appointment>> GetAppointments(int? month, int? week, Guid? instructorId);
    Task<List<Vehicle>> GetAllVehicles();
    Task<List<Candidate>> GetAllCadidates();


}
