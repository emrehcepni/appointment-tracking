using AppointmentTracking.Domain.Entities;
using AppointmentTracking.Infrastructure.Repositories.Interfaces;
using AppointmentTracking.Services.Interfaces;

namespace AppointmentTracking.Services;

public class VehicleService : IVehicleService
{
    private IVehicleRepository _vehicleRepository;

    public VehicleService(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }
    
    public async Task<Vehicle?> GetVehicleById(Guid vehicleDetailId)
    {
        var vehicleDetails = await _vehicleRepository.GetById(vehicleDetailId);
        return vehicleDetails;
    }
}
