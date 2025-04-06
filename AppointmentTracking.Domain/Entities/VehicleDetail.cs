using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppointmentTracking.Domain.Entities;

public class VehicleDetail : Entity<Guid>
{
    [ForeignKey("Vehicle")]
    public Guid VehicleId { get; set; }
    public string PlateNumber { get; set; }
    public Vehicle Vehicle { get; set; }
    
    public string Color { get; set; } // renk
    public string TransmissionType { get; set; } // Otomatik-Manuel Tipi
    public string TireSize { get; set; } //Lastik ebatları
    public string TireType { get; set; }//Lastik Tipleri
    public DateTime? TireChangeDate { get; set; } // Lastik değişim tarihi Opsiyonel
    public int? TireChangeKilometer { get; set; } // Lastik değişim km Opsiyonel
    public double EngineVolume { get; set; } //motor hacmi
    public int EnginePower { get; set; } //motor beygiri
    public string FuelType { get; set; } // yakıt tipi
    public string ChassisNumber { get; set; } //şasi numarası
    public DateTime InspectionDate { get; set; } //araç muayene tarihi
    public DateTime ExhaustInspectionDate { get; set; } //egzoz muayene tarihi
    public DateTime? InsuranceDate { get; set; } //Kasko tarihi Opsiyonel
    public string? Description { get; set; }//açıklama
}
