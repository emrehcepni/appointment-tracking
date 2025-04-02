namespace AppointmentTracking.Domain.Entities;

public class Candidate : Entity<Guid>
{
    public int CandidateId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public int LessonsHour { get; set; }
    public bool IsDeleted { get; set; }
    public List<Appointment>? Appointments { get; set; }
}
