namespace AppointmentTracking.Domain.Entities;

public abstract class Entity<T>
{
    public T Id { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedId { get; set; }

    public bool IsDeleted { get; set; }
}
