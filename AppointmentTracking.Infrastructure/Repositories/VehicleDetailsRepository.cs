using AppointmentTracking.Domain.Entities;
using AppointmentTracking.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppointmentTracking.Infrastructure.Repositories;

public class VehicleDetailsRepository : GenericRepository<VehicleDetail, Guid>, IVehicleDetailsRepository
{
    private readonly AppDbContext _dbContext;

    public VehicleDetailsRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<VehicleDetail?> GetVehicleWithDetails(Guid vehicleId)
    {
        var result = _dbContext.VehicleDetails.Include(vd => vd.Vehicle) 
            .FirstOrDefault(vd => vd.Id == vehicleId && !vd.IsDeleted);
        return result;
    }
}
