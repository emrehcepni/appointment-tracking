using AppointmentTracking.Domain.Entities;

namespace AppointmentTracking.Services.Interfaces;

public interface IVehicleDetailsService
{
    Task<VehicleDetail?> GetVehicleDetailById(Guid vehicleDetailId);
    Task<bool> AddVehicleDetail(VehicleDetail vehicleDetail);
    Task UpdateVehicleDetail(VehicleDetail vehicleDetail);
}
