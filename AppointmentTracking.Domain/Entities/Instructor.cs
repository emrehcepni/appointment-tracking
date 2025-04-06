namespace AppointmentTracking.Domain.Entities;

public class Instructor : Entity<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string LicanceType { get; set; }
    public List<Appointment> Appointments { get; set; }
}
