using AppointmentTracking.Domain.Entities;
using AppointmentTracking.Infrastructure.Repositories.Interfaces;
using AppointmentTracking.Services.Interfaces;

namespace AppointmentTracking.Services;

public class VehicleDetailsService : IVehicleDetailsService
{
    private IVehicleDetailsRepository _vehicleDetailsRepository;

    public VehicleDetailsService(IVehicleDetailsRepository vehicleDetailsRepository)
    {
        _vehicleDetailsRepository = vehicleDetailsRepository;
    }
    
    public async Task<VehicleDetail?> GetVehicleDetailById(Guid vehicleDetailId)
    {
        var vehicleDetails = await _vehicleDetailsRepository.GetVehicleWithDetails(vehicleDetailId);
        return vehicleDetails;
    }
    
    public async Task<bool> AddVehicleDetail(VehicleDetail vehicleDetail)
    {
        var result = await _vehicleDetailsRepository.CreateAsync(vehicleDetail);
        return result.Id != Guid.Empty;
    }
    
    public async Task UpdateVehicleDetail(VehicleDetail vehicleDetail)
    {
        await _vehicleDetailsRepository.UpdateAsync(vehicleDetail);
    }
}
