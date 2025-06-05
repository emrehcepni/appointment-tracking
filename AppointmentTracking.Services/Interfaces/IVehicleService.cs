using AppointmentTracking.Domain.Entities;

namespace AppointmentTracking.Services.Interfaces;

public interface IVehicleService
{
    Task<IEnumerable<Vehicle>> GetAllVehicles();
    Task<bool> DeleteVehicle(Guid vehicleId);
    Task<List<Vehicle>> SearchVehicle(string searchString);
    Task<bool> AddVehicle(Vehicle vehicle);
    Task<Vehicle?> GetVehicleById(Guid vehicleId);
    Task UpdateVehicle(Vehicle vehicle);


    Task<Vehicle?> GetVehicleDetailById(Guid vehicleDetailId);
}
