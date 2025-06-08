using AppointmentTracking.Domain.Entities;
using AppointmentTracking.Infrastructure.Repositories.Interfaces;
using AppointmentTracking.Services.Interfaces;

namespace AppointmentTracking.Services;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleService(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<List<Vehicle>> GetAllVehicles()
    {
        var vehicles = await _vehicleRepository.WhereWithAsNoTrackingAsync(v => !v.IsDeleted);
        return vehicles.ToList();
    }

    public Task<Vehicle?> GetVehicleById(Guid vehicleId)
    {
        return _vehicleRepository.FirstOrDefaultAsync(v => v.Id == vehicleId);
    }

    public async Task<bool> AddVehicle(Vehicle vehicle)
    {
        await _vehicleRepository.CreateAsync(vehicle);
        return true;
    }

    public async Task<bool> DeleteVehicle(Guid vehicleId)
    {
        var vehicle = await _vehicleRepository.FirstOrDefaultAsync(v => v.Id == vehicleId);
        if (vehicle == null) return false;

        vehicle.IsDeleted = true;
        await _vehicleRepository.UpdateAsync(vehicle);
        return true;
    }

    public async Task<List<Vehicle>> SearchVehicle(string searchString)
    {
        var vehicles = await _vehicleRepository.WhereWithAsNoTrackingAsync(v =>
            v.LicensePlate.Contains(searchString) && !v.IsDeleted);
        return vehicles.ToList();
    }

    public async Task UpdateVehicle(Vehicle vehicle)
    {
        await _vehicleRepository.UpdateAsync(vehicle);
    }

    public async Task<Vehicle?> GetVehicleDetailById(Guid vehicleDetailId)
    {
        return await _vehicleRepository.FirstOrDefaultWithAsNoTrackingAsync(v => v.Id == vehicleDetailId);
    }
}

