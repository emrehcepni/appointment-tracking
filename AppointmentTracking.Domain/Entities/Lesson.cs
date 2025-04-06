namespace AppointmentTracking.Domain.Entities;

public class Lesson : Entity<Guid>
{
    public string name { get; set; }
    public string lessonDateTime { get; set; }
}
