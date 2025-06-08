using Microsoft.AspNetCore.Mvc;
using AppointmentTracking.Domain.Entities;
using AppointmentTracking.Services.Interfaces;

namespace AppointmentTracking.Controllers;

public class VehicleController : Controller
{
    private readonly IVehicleService _vehicleService;

    public VehicleController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    // GET: Vehicle/Index
    public async Task<IActionResult> Index(int? page)
    {
        ViewBag.ActiveMenuItem = "Araçlar";
        int pageSize = 5;
        int pageNumber = page ?? 1;

        var vehicles = await _vehicleService.GetAllVehicles();
        var paged = vehicles
            .Where(v => !v.IsDeleted)
            .OrderBy(v => v.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.TotalPages = (int)Math.Ceiling(vehicles.Count / (double)pageSize);
        ViewBag.CurrentPage = pageNumber;

        return View("Index", paged);
    }

    // POST: Vehicle/AddOrUpdate
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddOrUpdate(Vehicle vehicle)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Lütfen tüm alanları doldurun.";
            return RedirectToAction("Index");
        }

        if (vehicle.Id == Guid.Empty)
        {
            vehicle.Id = Guid.NewGuid();
            await _vehicleService.AddVehicle(vehicle);
            TempData["SuccessMessage"] = "Araç başarıyla eklendi.";
        }
        else
        {
            await _vehicleService.UpdateVehicle(vehicle);
            TempData["SuccessMessage"] = "Araç başarıyla güncellendi.";
        }

        return RedirectToAction("Index");
    }

    // GET: Vehicle/Search
    [HttpGet]
    public async Task<IActionResult> Search(string? make, string? brand, int? modelYear, string? licensePlate, DateTime? inspectionDate, bool? accessible, int? page)
    {
        int pageSize = 5;
        int pageNumber = page ?? 1;

        var vehicles = await _vehicleService.SearchVehicle("");
        vehicles = vehicles
            .Where(v => !v.IsDeleted &&
                (string.IsNullOrEmpty(make) || v.Make.Contains(make)) &&
                (string.IsNullOrEmpty(brand) || v.Brand.Contains(brand)) &&
                (!modelYear.HasValue || v.ModelYear == modelYear.Value) &&
                (string.IsNullOrEmpty(licensePlate) || v.LicensePlate.Contains(licensePlate)) &&
                (!inspectionDate.HasValue || v.InspectionDate == inspectionDate.Value) &&
                (!accessible.HasValue || v.Accessible == accessible.Value))
            .ToList();

        var pagedVehicles = vehicles
            .OrderByDescending(v => v.CreatedDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.TotalPages = (int)Math.Ceiling(vehicles.Count / (double)pageSize);
        ViewBag.CurrentPage = pageNumber;

        return View("Index", pagedVehicles);
    }

    // POST: Vehicle/Delete
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid selectedVehicleId)
    {
        var vehicle = await _vehicleService.GetVehicleById(selectedVehicleId);
        if (vehicle == null)
        {
            TempData["ErrorMessage"] = "Araç bulunamadı.";
            return RedirectToAction("Index");
        }

        vehicle.IsDeleted = true;
        await _vehicleService.UpdateVehicle(vehicle);
        TempData["SuccessMessage"] = "Araç başarıyla silindi.";
        return RedirectToAction("Index");
    }

    // GET: Vehicle/CheckInspectionStatus
    public async Task<IActionResult> CheckInspectionStatus()
    {
        var today = DateTime.Today;
        var warningDate = today.AddDays(10);

        var vehicles = await _vehicleService.GetAllVehicles();
        var expiring = vehicles
            .Where(v => v.InspectionDate.HasValue && v.InspectionDate.Value <= warningDate)
            .ToList();

        return Json(expiring);
    }
}
