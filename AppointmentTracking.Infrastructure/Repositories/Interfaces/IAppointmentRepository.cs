using AppointmentTracking.Domain.Entities;

namespace AppointmentTracking.Infrastructure.Repositories.Interfaces;

public interface IAppointmentRepository : IGenericRepository<Appointment, Guid>
{

}
