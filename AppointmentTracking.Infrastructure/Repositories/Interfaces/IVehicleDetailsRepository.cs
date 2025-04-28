using AppointmentTracking.Domain.Entities;

namespace AppointmentTracking.Infrastructure.Repositories.Interfaces;

public interface IVehicleDetailsRepository : IGenericRepository<VehicleDetail, Guid>
{
    Task<VehicleDetail?> GetVehicleWithDetails(Guid vehicleId);
}
