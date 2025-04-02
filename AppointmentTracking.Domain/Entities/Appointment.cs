namespace AppointmentTracking.Domain.Entities;

public class Appointment : Entity<Guid>
{

    public int AppointmentId { get; set; }
    public int InstructorId { get; set; }
    public Instructor? Instructor { get; set; }

    public int CandidateId { get; set; }
    public Candidate? Candidate { get; set; }

    public int VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }
    public virtual ICollection<Vehicle>? Vehicles { get; set; }

    public DateTime StartTime { get; set; } // Dersin başlangıç zamanı
    public DateTime EndTime { get; set; }   // Dersin bitiş zamanı
}

