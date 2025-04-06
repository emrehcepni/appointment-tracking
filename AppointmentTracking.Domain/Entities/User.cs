using System.ComponentModel.DataAnnotations;

namespace AppointmentTracking.Domain.Entities;

public class User : Entity<Guid>
{
    public string userName {  get; set; }
    public string password { get; set; }
    public string email { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string userType { get; set; }
    
}
