using System.ComponentModel.DataAnnotations;

namespace AppointmentTracking.Domain.Entities;

public class Users : Entity<Guid>
{
    [Key]    
    public int UserId {  get; set; }
    public string userName {  get; set; }
    public string password { get; set; }
    public string email { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string userType { get; set; }
    
}
