using Microsoft.AspNetCore.Mvc;
using AppointmentTracking.Domain.Entities;
using AppointmentTracking.Services.Interfaces;

namespace AppointmentTracking.Controllers;

public class VehicleDetailsController : Controller
{
    private readonly IVehicleService _vehicleService;
    private readonly IVehicleDetailsService _vehicleDetailsService;
    
    public VehicleDetailsController(IVehicleDetailsService vehicleDetailsService, IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
        _vehicleDetailsService = vehicleDetailsService;
    }

    public async Task<IActionResult> Details(Guid vehicleId)
    {
        var vehicleDetail = await _vehicleDetailsService.GetVehicleDetailById(vehicleId);
        if (vehicleDetail is null)
            return NotFound();

        return View(vehicleDetail);
    }

    public async Task<IActionResult> Index(Guid vehicleId)
    {
        var vehicle = await _vehicleService.GetVehicleById(vehicleId);
        if (vehicle is null)
            return NotFound();

        var vehicleDetail = await _vehicleDetailsService.GetVehicleDetailById(vehicleId);
        if (vehicleDetail is null)
        {
            // Eğer detay bilgisi yoksa, yeni bir form oluştur
            vehicleDetail = new VehicleDetail
            {
                VehicleId = vehicle.Id,
                PlateNumber = vehicle.LicensePlate // Plaka otomatik atanıyor
            };
        }

        return View(vehicleDetail);
    }

    [HttpPost]
    public async Task<IActionResult> Create(VehicleDetail vehicleDetail)
    {
        ModelState.Clear();
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Lütfen tüm alanları doğru doldurduğunuzdan emin olun.";
            return View("Index", vehicleDetail);
        }
        
        var existingDetail = await _vehicleDetailsService.GetVehicleDetailById(vehicleDetail.VehicleId);
        if (existingDetail is null)
        {
            await _vehicleDetailsService.AddVehicleDetail(vehicleDetail);
            return RedirectToAction("Index", new { vehicleId = vehicleDetail.VehicleId });
        }
        
        existingDetail.Color = vehicleDetail.Color;
        existingDetail.TransmissionType = vehicleDetail.TransmissionType;
        existingDetail.FuelType = vehicleDetail.FuelType;
        existingDetail.EngineVolume = vehicleDetail.EngineVolume;
        existingDetail.EnginePower = vehicleDetail.EnginePower;
        existingDetail.Description = vehicleDetail.Description;
        await _vehicleDetailsService.UpdateVehicleDetail(existingDetail);

        return RedirectToAction("Index", new { vehicleId = vehicleDetail.VehicleId });
    }
}
