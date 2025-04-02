using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppointmentTracking.Infrastructure;
using AppointmentTracking.Domain.Entities;

namespace AppointmentTracking.Controllers;//kontrol
public class VehicleDetailController : Controller
{
    private readonly AppDbContext _context;

    public VehicleDetailController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Details(int id)
    {
        var vehicleDetail = _context.VehicleDetails
            .Include(vd => vd.Vehicle) // Araç bilgilerini çekiyoruz
            .FirstOrDefault(vd => vd.VehicleId == id);

        if (vehicleDetail == null)
        {
            return NotFound();
        }

        return View(vehicleDetail);
    }

    public IActionResult Index(int vehicleId)
    {
        var vehicle = _context.Vehicles.FirstOrDefault(v => v.VehicleId == vehicleId);

        if (vehicle == null)
        {
            return NotFound();
        }

        // Aracın detay bilgisi var mı kontrol et
        var vehicleDetail = _context.VehicleDetails
            .FirstOrDefault(vd => vd.VehicleId == vehicleId);

        if (vehicleDetail == null)
        {
            // Eğer detay bilgisi yoksa, yeni bir form oluştur
            vehicleDetail = new VehicleDetails
            {
                VehicleId = vehicle.VehicleId,
                PlateNumber = vehicle.LicensePlate // Plaka otomatik atanıyor
            };
        }

        return View(vehicleDetail);
    }


    [HttpPost]
    public IActionResult Create(VehicleDetails vehicleDetail)
    {
        

        ModelState.Clear();
            if (ModelState.IsValid)
            {
                var existingDetail = _context.VehicleDetails
                    .FirstOrDefault(vd => vd.VehicleId == vehicleDetail.VehicleId);

                if (existingDetail == null)
                {
                    // Yeni kayıt ekle
                    _context.VehicleDetails.Add(vehicleDetail);
                }
                else
                {
                    // Güncelleme işlemi
                    existingDetail.Color = vehicleDetail.Color;
                    existingDetail.TransmissionType = vehicleDetail.TransmissionType;
                    existingDetail.FuelType = vehicleDetail.FuelType;
                    existingDetail.EngineVolume = vehicleDetail.EngineVolume;
                    existingDetail.EnginePower = vehicleDetail.EnginePower;
                    existingDetail.Description = vehicleDetail.Description;
                }

                _context.SaveChanges();

                return RedirectToAction("Index", new { vehicleId = vehicleDetail.VehicleId });
            }

            TempData["ErrorMessage"] = "Lütfen tüm alanları doğru doldurduğunuzdan emin olun.";
            return View("Index", vehicleDetail);
        





    }
}
