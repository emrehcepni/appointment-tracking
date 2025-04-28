using AppointmentTracking.Domain.Entities;

namespace AppointmentTracking.Services.Interfaces;

public interface IVehicleService
{
    Task<Vehicle?> GetVehicleById(Guid vehicleDetailId);
}
