using System.ComponentModel.DataAnnotations;

namespace AppointmentTracking.Domain.Entities;

public class Vehicle : Entity<Guid>
{
    [Required]
    public int VehicleId { get; set; }
    [Required]
    public string Make { get; set; }
    [Required]
    public string Brand { get; set; }
    public bool Accessible { get; set; }
    [Required]
    public int ModelYear { get; set; }
    [Required]
    public string LicensePlate { get; set; }
    public VehicleDetails? VehicleDetails { get; set; }
    public DateTime? InspectionDate {  get; set; }
    public List<Appointment>? Appointments { get; set; }
    public bool IsDeleted { get; set; }
}
